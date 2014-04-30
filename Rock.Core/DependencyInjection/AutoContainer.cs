using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Rock.DependencyInjection.Heuristics;

namespace Rock.DependencyInjection
{
    public partial class AutoContainer : IResolver
    {
        private readonly Lazy<MethodInfo> _getMethod;
        private readonly ConcurrentDictionary<Type, Func<object>> _bindings;
        private readonly IConstructorSelector _constructorSelector;

        private AutoContainer(
            ConcurrentDictionary<Type, Func<object>> bindings,
            IConstructorSelector constructorSelector)
        {
            _getMethod = new Lazy<MethodInfo>(() => GetType().GetMethod("Get", Type.EmptyTypes));
            _constructorSelector = constructorSelector;
            _bindings = bindings;
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        protected AutoContainer(AutoContainer parentContainer)
            : this(parentContainer._bindings, parentContainer._constructorSelector)
        {
        }

        public AutoContainer(params object[] instances)
            : this(new ConcurrentDictionary<Type, Func<object>>(), new ConstructorSelector())
        {
            foreach (var instance in instances.Where(x => x != null))
            {
                var localInstance = instance;
                foreach (var type in GetTypeHierarchy(instance.GetType()))
                {
                    if (_bindings.ContainsKey(type))
                    {
                        _bindings[type] = null;
                    }
                    else
                    {
                        _bindings[type] = () => localInstance;
                    }
                }
            }
        }

        public virtual bool CanResolve(Type type)
        {
            Func<object> binding;
            return
                (_bindings.TryGetValue(type, out binding) && binding != null)
                || CanGetConstructor(type);
        }

        public T Get<T>()
        {
            var instance = Get(typeof(T));

            if (instance == null)
            {
                return default(T);
            }

            return (T)instance;
        }

        public virtual object Get(Type type)
        {
            var getInstance =
                _bindings.GetOrAdd(
                    type,
                    t =>
                        CanGetConstructor(type)
                            ? GetCreateInstanceFunc(type)
                            : null);

            if (getInstance == null)
            {
                return null;
            }

            return getInstance();
        }

        IResolver IResolver.MergeWith(IResolver otherContainer)
        {
            return MergeWith(otherContainer);
        }

        public virtual AutoContainer MergeWith(IResolver otherContainer)
        {
            return new MergedAutoContainer(this, otherContainer);
        }

        private bool CanGetConstructor(Type type)
        {
            return
                type != typeof(object)
                && _constructorSelector.CanGetConstructor(type, this);
        }

        private Func<object> GetCreateInstanceFunc(Type type)
        {
            ConstructorInfo ctor;
            if (!_constructorSelector.TryGetConstructor(type, this, out ctor))
            {
                return null;
            }

            var thisExpression = Expression.Constant(this);

            var createInstanceExpression =
                Expression.Lambda<Func<object>>(
                    Expression.New(
                        ctor,
                        ctor.GetParameters()
                            .Select(
                                p =>
                                CanResolve(p.ParameterType)
                                    ? (Expression)Expression.Call(
                                        thisExpression,
                                        _getMethod.Value.MakeGenericMethod(p.ParameterType))
                                    : Expression.Constant(p.DefaultValue, p.ParameterType))));

            return createInstanceExpression.Compile();
        }

        private static IEnumerable<Type> GetTypeHierarchy(Type type)
        {
            return GetConcreteHierarchy(type).Concat(type.GetInterfaces());
        }

        private static IEnumerable<Type> GetConcreteHierarchy(Type type)
        {
            while (type != null && type != typeof(object))
            {
                yield return type;
                type = type.BaseType;
            }
        }
    }
}