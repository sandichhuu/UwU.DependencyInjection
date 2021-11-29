using System;
using System.Collections.Generic;
using UwU.UnityBridge;

namespace UwU.DI.Binding
{
    using UwU.DI.Collection;
    using UwU.DI.Container;
    using UwU.DI.GC;
    using UwU.Log;

    public class Binder : IBinder, IDisposable
    {
        private static readonly string[] IgnoreNamespace = new[] { "System", "UnityEngine" };

        private readonly ILogger logger;
        private readonly IDependencyContainer container;
        private readonly bool useMultiThread;

        public IReadOnlyCollection<BindingCommand> bindingCommands { get; private set; }

        public Binder(IDependencyContainer container, ILogger logger, bool useMultiThread = false)
        {
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
            BindRelevantsTypeCommand(instance, IgnoreNamespace);
        }

        public void BindGameObjectRelevantsTypeCommand<ComponentType>(string gameObjectName)
        {
            BindGameObjectRelevantsTypeCommand<ComponentType>(gameObjectName, IgnoreNamespace);
        }

        public void BindGameObjectRelevantsTypeCommand<ComponentType>(string gameObjectName, string[] ignoreNamespaceList)
        {
            var objectHolder = GameObject.Find(gameObjectName);
            var component = objectHolder.GetComponent<ComponentType>();

            BindRelevantsTypeCommand(component, ignoreNamespaceList);
        }

        public void BindRelevantsTypeCommand(object instance, string[] ignoreNamespaceList)
        {
            var instanceType = instance.GetType();

            foreach (var type in GetRelevantTypes(instanceType, ignoreNamespaceList))
            {
                var typeHash = type.GetHashCode();

                var bindingCommand = new BindingCommand
                {
                    instaceHandle = new ObjectHandler(instance),
                    sourceTypeHash = typeHash,
                    targetTypeHash = typeHash
                };

                this.bindingCommands.Add(bindingCommand);

                this.logger?.Trace($"BindRelevants SourceType[{type.Name}] -> TargetType[{instanceType.Name}] -> [{instance.GetHashCode()}]");
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

            var bindingCommand = new BindingCommand
            {
                instaceHandle = new ObjectHandler(instance),
                sourceTypeHash = sourceType.GetHashCode(),
                targetTypeHash = targetType.GetHashCode()
            };

            this.bindingCommands.Add(bindingCommand);

            this.logger?.Trace($"Bind SourceType[{sourceType.Name}] -> TargetType[{targetType.Name}] -> [{instance.GetHashCode()}]");
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
            var component = objectHolder.GetComponent<SourceType>();

            var bindingCommand = new BindingCommand
            {
                instaceHandle = new ObjectHandler(component),
                sourceTypeHash = sourceType.GetHashCode(),
                targetTypeHash = targetType.GetHashCode()
            };

            this.bindingCommands.Add(bindingCommand);

            this.logger?.Trace($"BindGameObject SourceType[{sourceType.Name}] -> TargetType[{targetType.Name}] -> [{component.GetHashCode()}]");
        }

        public void Unbind<T>()
        {
            this.logger?.Trace($"UnbindType [{typeof(T).Name}]");
            this.container.RemoveType<T>();
        }

        public void Unbind<T>(T obj)
        {
            this.logger?.Trace($"UnbindObject [{obj.GetType().Name}] -> [{obj.GetHashCode()}]");
            this.container.RemoveInstance(obj);
        }

        public void ExecuteBindingCommand()
        {
            this.bindingCommands.ForEach(command =>
            {
                using (command.instaceHandle)
                {
                    this.container.AddDirect(command.sourceTypeHash, command.targetTypeHash, command.instaceHandle.handle.Target);
                }
            });

            ReinitCommandContainer();
        }

        public void Dispose()
        {
            this.bindingCommands?.Dispose();
        }

        private IEnumerable<Type> GetRelevantTypes(Type type, string[] ignoreList)
        {
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