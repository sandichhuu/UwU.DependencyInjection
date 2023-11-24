using System;
using System.Collections.Generic;

using UnityEngine;
using Object = UnityEngine.Object;

namespace UwU.DI.Binding
{
    using UwU.DI.Collection;
    using UwU.DI.Container;
    using UwU.DI.GC;
    using UwU.Core;

    public class Binder : IBinder, IDisposable
    {
        private readonly ILogger logger;
        private readonly IDependencyContainer container;

        public ICollection<BindingCommand> bindingCommands { get; private set; }

        public Binder(IDependencyContainer container, ILogger logger)
        {
            this.logger = logger;
            this.container = container;

            ReinitCommandContainer();
        }

        private void ReinitCommandContainer()
        {
            if (this.bindingCommands != null)
            {
                this.bindingCommands.Dispose();
            }

            this.bindingCommands = new ReadOnlyCollection<BindingCommand>(64);
        }

        public void BindRelevantsTypeCommand(object instance)
        {
            var instanceType = instance.GetType();

            foreach (var type in GetRelevantTypes(instanceType, new string[] { }))
            {
                var typeHash = type.GetId();

                var bindingCommand = new BindingCommand
                {
                    instaceHandle = new ObjectHandler(instance),
                    sourceTypeHash = typeHash,
                    targetTypeHash = typeHash
                };

                this.bindingCommands.Add(bindingCommand);

                if (this.logger != null)
                {
                    this.logger.Trace("BindRelevants SourceType[" + type.Name + "] -> TargetType[" + instanceType.Name + "] -> [" + instance.GetHashCode() + "]");
                }
            }
        }

        public void BindComponentRelevantsCommand<FindComponentType>() where FindComponentType : Object
        {
            var component = Object.FindObjectOfType<FindComponentType>();
            BindRelevantsTypeCommand(component);
        }

        public void BindComponentRelevantsCommand<FindComponentType>(string[] ignoreNamespaceList) where FindComponentType : Object
        {
            var component = Object.FindObjectOfType<FindComponentType>();
            BindRelevantsTypeCommand(component, ignoreNamespaceList);
        }

        public void BindGameObjectRelevantsTypeCommand<FindComponentType>(string gameObjectName)
        {
            BindGameObjectRelevantsTypeCommand<FindComponentType>(gameObjectName);
        }

        public void BindGameObjectRelevantsTypeCommand<FindComponentType>(string gameObjectName, string[] ignoreNamespaceList)
        {
            var objectHolder = UnityEngine.GameObject.Find(gameObjectName);
            var component = objectHolder.GetComponent<FindComponentType>();

            BindRelevantsTypeCommand(component, ignoreNamespaceList);
        }

        public void BindRelevantsTypeCommand(object instance, string[] ignoreNamespaceList)
        {
            var instanceType = instance.GetType();

            foreach (var type in GetRelevantTypes(instanceType, ignoreNamespaceList))
            {
                var typeHash = type.GetId();

                var bindingCommand = new BindingCommand
                {
                    instaceHandle = new ObjectHandler(instance),
                    sourceTypeHash = typeHash,
                    targetTypeHash = typeHash
                };

                this.bindingCommands.Add(bindingCommand);
            }
        }

        public void BindCommand<SourceType>(SourceType sourceType)
        {
            BindCommand<SourceType, SourceType>(sourceType);
        }

        public void BindCommand<SourceType, TargetType>(TargetType instance)
        {
            var sourceType = typeof(SourceType);
            var targetType = typeof(TargetType);

            if (instance != null)
            {
                var bindingCommand = new BindingCommand
                {
                    instaceHandle = new ObjectHandler(instance),
                    sourceTypeHash = sourceType.GetId(),
                    targetTypeHash = targetType.GetId()
                };

                this.bindingCommands.Add(bindingCommand);
            }
            else
            {
                throw new Exception("Bind " + typeof(TargetType).Name + " failed !");
            }
        }

        public void BindComponentCommand<SourceType>() where SourceType : Object
        {
            BindCommand<SourceType, SourceType>(Object.FindObjectOfType<SourceType>());
        }

        public void BindComponentCommand<SourceType, TargetType>() where TargetType : Object
        {
            BindCommand<SourceType, TargetType>(Object.FindObjectOfType<TargetType>());
        }

#if UNITY_2020_1_OR_NEWER
        public void BindComponentCommand<SourceType>(bool includeInactive) where SourceType : Object
        {
            BindCommand<SourceType, SourceType>(Object.FindObjectOfType<SourceType>(includeInactive));
        }

        public void BindComponentCommand<SourceType, TargetType>(bool includeInactive) where TargetType : Object
        {
            BindCommand<SourceType, TargetType>(Object.FindObjectOfType<TargetType>(includeInactive));
        }
#else
        public void BindComponentCommand<SourceType>(bool includeInactive) where SourceType : Object
        {
            BindCommand<SourceType, SourceType>(Object.FindObjectOfType<SourceType>());
        }

        public void BindComponentCommand<SourceType, TargetType>(bool includeInactive) where TargetType : Object
        {
            BindCommand<SourceType, TargetType>(Object.FindObjectOfType<TargetType>());
        }
#endif

        public void BindGameObjectCommand<SourceType>(string gameObjectName)
        {
            BindGameObjectCommand<SourceType, SourceType>(gameObjectName);
        }

        public void BindGameObjectCommand<SourceType, TargetType>(string gameObjectName)
        {
            var sourceType = typeof(SourceType);
            var targetType = typeof(TargetType);

            var objectHolder = GameObject.Find(gameObjectName);
            var component = objectHolder.GetComponent<TargetType>();

            var bindingCommand = new BindingCommand
            {
                instaceHandle = new ObjectHandler(component),
                sourceTypeHash = sourceType.GetId(),
                targetTypeHash = targetType.GetId()
            };

            this.bindingCommands.Add(bindingCommand);

            if (this.logger != null)
            {
                this.logger.Trace("BindGameObject SourceType[" + sourceType.Name + "] -> TargetType[" + targetType.Name + "] -> [" + component.GetHashCode() + "]");
            }
        }

        public void Unbind<T>()
        {
            if (this.logger != null)
            {
                this.logger.Trace("UnbindType [" + typeof(T).Name + "]");
            }
            this.container.RemoveType<T>();
        }

        public void Unbind<T>(T obj)
        {
            if (this.logger != null)
            {
                this.logger.Trace("UnbindObject [" + obj.GetType().Name + "] -> [" + obj.GetHashCode() + "]");
            }
            this.container.RemoveInstance(obj);
        }

        public void ExecuteBindingCommand()
        {
            this.bindingCommands.ForEach(command =>
            {
                this.container.AddDirect(command.sourceTypeHash, command.targetTypeHash, command.instaceHandle.handle.Target);
            });

            ReinitCommandContainer();
        }

        public void Dispose()
        {
            if (this.bindingCommands != null)
            {
                this.bindingCommands.Dispose();
            }
        }

        private bool IsIgnore(string[] ignoreList, string namespaceString)
        {
            var ignore = false;
            for (var i = 0; i < ignoreList.Length; i++)
            {
                if (namespaceString.StartsWith(ignoreList[i]))
                {
                    ignore = true;
                    break;
                }
            }

            return ignore;
        }

        private IEnumerable<Type> GetRelevantTypes(Type type, string[] ignoreList)
        {
            yield return type;

            foreach (var implementedInterface in type.GetInterfaces())
            {
                if (IsIgnore(ignoreList, implementedInterface.Namespace))
                {
                    continue;
                }

                yield return implementedInterface;
            }

            var currentBaseType = type.BaseType;
            while (currentBaseType != null)
            {
                if (IsIgnore(ignoreList, currentBaseType.Namespace))
                {
                    currentBaseType = currentBaseType.BaseType;
                    continue;
                }

                yield return currentBaseType;
                currentBaseType = currentBaseType.BaseType;
            }
        }
    }
}