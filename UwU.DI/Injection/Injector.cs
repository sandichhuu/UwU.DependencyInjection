using System;
using System.Reflection;

namespace UwU.DI.Injection
{
    using UwU.DI.Container;
    using UwU.Core;

    public class Injector : IInjector
    {
        private readonly IDependencyContainer container;
        private readonly ILogger logger;

        public Injector(IDependencyContainer container)
        {
            this.container = container;
        }

        public Injector(IDependencyContainer container, ILogger logger) : this(container)
        {
            this.logger = logger;
        }

        public void Inject(object target)
        {
            var type = target.GetType();

            var fields = type.GetFields(BindingFlags.Instance |
                             BindingFlags.Static |
                             BindingFlags.Public |
                             BindingFlags.NonPublic);

            var properties = type.GetProperties(BindingFlags.Instance |
                             BindingFlags.Static |
                             BindingFlags.Public |
                             BindingFlags.NonPublic);

            var methods = type.GetMethods(BindingFlags.Instance |
                             BindingFlags.Static |
                             BindingFlags.Public |
                             BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                if (Attribute.IsDefined(field, typeof(Inject)))
                {
                    InjectField(target, field);
                }
            }

            foreach (var property in properties)
            {
                if (Attribute.IsDefined(property, typeof(Inject)))
                {
                    InjectProperty(target, property);
                }
            }

            foreach (var method in methods)
            {
                if (Attribute.IsDefined(method, typeof(Inject)))
                {
                    InjectMethod(target, method);
                }
            }
        }

        private void InjectField(object target, FieldInfo fieldInfo)
        {
            var instance = container.First(fieldInfo.FieldType);

            if (instance != null)
            {
                fieldInfo.SetValue(target, instance);
            }
            else
            {
                if (logger != null)
                {
                    logger.Error("Inject " + target.GetType().Name + "->" + fieldInfo.Name + " failed !");
                }
            }
        }

        private void InjectProperty(object target, PropertyInfo propertyInfo)
        {
            var instance = container.First(propertyInfo.PropertyType);

            if (instance != null)
            {
                propertyInfo.SetValue(target, instance, null);
            }
            else
            {
                if (logger != null)
                {
                    logger.Error("Inject " + target.GetType().Name + "->" + propertyInfo.Name + " failed !");
                }
            }
        }

        private void InjectMethod(object target, MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            var length = parameters.Length;
            var executeParamValues = new object[length];

            for (var i = 0; i < length; i++)
            {
                var param = parameters[i];
                var instance = container.First(param.ParameterType);

                if (instance != null)
                {
                    executeParamValues[i] = instance;
                }
                else
                {
                    executeParamValues[i] = null;
                    if (logger != null)
                    {
                        logger.Warn("Inject " + target.GetType().Name + "->" + methodInfo.Name + "->" + param.Name + " failed !");
                    }
                }
            }

            methodInfo.Invoke(target, executeParamValues);
        }
    }
}