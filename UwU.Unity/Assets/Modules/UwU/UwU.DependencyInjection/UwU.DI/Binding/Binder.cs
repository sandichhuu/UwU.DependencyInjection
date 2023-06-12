using System;
using System.Collections.Generic;

using UnityEngine;
using Object = UnityEngine.Object;

namespace UwU.DI.Binding
{
    using UwU.DI.Collection;
    using UwU.DI.Container;
    using UwU.DI.GC;
    using UwU.Logger;
    using UwU.TypeId;

    public class Binder : IBinder, IDisposable
    {
        private readonly ILogger logger;
        private readonly IDependencyContainer container;
        private readonly bool useMultiThread;
        private readonly IdProvider idProvider;
        public IReadOnlyCollection<BindingCommand> bindingCommands { get; private set; }

        public Binder(IdProvider idProvider, IDependencyContainer container, ILogger logger, bool useMultiThread = false)
        {
            this.idProvider = idProvider;
            this.logger = logger;
            this.container = container;
            this.useMultiThread = useMultiThread;

            ReinitCommandContainer();
        }

        private void ReinitCommandContainer()
        {
            this.bindingCommands?.Dispose();

            if (this.useMultiThread)
            {
                this.bindingCommands = new MultiThreadReadOnlyCollection<BindingCommand>(64);
            }
            else
            {
                this.bindingCommands = new SingleThreadReadOnlyCollection<BindingCommand>(64);
            }
        }

        public void BindRelevantsTypeCommand(object instance)
        {
            var instanceType = instance.GetType();

            foreach (var type in GetRelevantTypes(instanceType, new string[] { }))
            {
                var typeHash = this.idProvider.GetId(type);

                var bindingCommand = new BindingCommand
                {
                    instaceHandle = new ObjectHandler(instance),
                    sourceTypeHash = typeHash,
                    targetTypeHash = typeHash
                };

                this.bindingCommands.Add(bindingCommand);

                this.logger?.Trace($"BindRelevants SourceType[{type.Name}] -> TargetType[{instanceType.Name}] -> [{this.idProvider.GetId(type)}]");
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
                var typeHash = this.idProvider.GetId(type);

                var bindingCommand = new BindingCommand
                {
                    instaceHandle = new ObjectHandler(instance),
                    sourceTypeHash = typeHash,
                    targetTypeHash = typeHash
                };

                this.bindingCommands.Add(bindingCommand);
                //this.logger?.Trace($"BindRelevants SourceType[{type.Name}] -> TargetType[{instanceType.Name}] -> [{this.idProvider.GetId(type)}]");
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
                    sourceTypeHash = this.idProvider.GetId(sourceType),
                    targetTypeHash = this.idProvider.GetId(targetType)
                };

                this.bindingCommands.Add(bindingCommand);
            }
            else
            {
                throw new Exception($"Bind {typeof(TargetType).Name} failed !");
            }

            //if (instance == null)
            //{
            //    this.logger?.Error($"Bind SourceType[{sourceType.Name}] -> TargetType[{targetType.Name}] [failed]");
            //}
            //else
            //{
            //    this.logger?.Trace($"Bind SourceType[{sourceType.Name}] -> TargetType[{targetType.Name}] -> [{this.idProvider.GetId(type)}] [OK]");
            //}
        }

        public void BindComponentCommand<SourceType>() where SourceType : Object
        {
            BindCommand<SourceType, SourceType>(Object.FindObjectOfType<SourceType>());
        }

        public void BindComponentCommand<SourceType, TargetType>() where TargetType : Object
        {
            BindCommand<SourceType, TargetType>(Object.FindObjectOfType<TargetType>());
        }

        public void BindComponentCommand<SourceType>(bool includeInactive) where SourceType : Object
        {
            BindCommand<SourceType, SourceType>(Object.FindObjectOfType<SourceType>(includeInactive));
        }

        public void BindComponentCommand<SourceType, TargetType>(bool includeInactive) where TargetType : Object
        {
            BindCommand<SourceType, TargetType>(Object.FindObjectOfType<TargetType>(includeInactive));
        }

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
                sourceTypeHash = this.idProvider.GetId(sourceType),
                targetTypeHash = this.idProvider.GetId(targetType)
            };

            this.bindingCommands.Add(bindingCommand);

            this.logger?.Trace($"BindGameObject SourceType[{sourceType.Name}] -> TargetType[{targetType.Name}] -> [{this.idProvider.GetId(targetType)}]");
        }

        public void Unbind<T>()
        {
            this.logger?.Trace($"UnbindType [{typeof(T).Name}]");
            this.container.RemoveType<T>();
        }

        public void Unbind<T>(T obj)
        {
            this.logger?.Trace($"UnbindObject [{obj.GetType().Name}] -> [{this.idProvider.GetId(obj.GetType())}]");
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
            this.bindingCommands?.Dispose();
        }

        private IEnumerable<Type> GetRelevantTypes(Type type, string[] ignoreList)
        {
            yield return type;

            bool IsIgnore(string namespaceString)
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

            foreach (var implementedInterface in type.GetInterfaces())
            {
                if (IsIgnore(implementedInterface.Namespace))
                {
                    continue;
                }

                yield return implementedInterface;
            }

            var currentBaseType = type.BaseType;
            while (currentBaseType != null)
            {
                if (IsIgnore(currentBaseType.Namespace))
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