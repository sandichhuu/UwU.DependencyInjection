using System;
using System.Collections.Generic;
using UwU.TypeId;

namespace UwU.DI.Container
{
    public class IDContainer : IDependencyContainer, IDisposable
    {
        private readonly IdProvider idProvider;
        private readonly IDictionary<int, object> objectContainer;
        private readonly IDictionary<int, IList<int>> dependencyContainer;

        public IDContainer(IdProvider idProvider)
        {
            this.idProvider = idProvider;
            this.objectContainer = new Dictionary<int, object>(64);
            this.dependencyContainer = new Dictionary<int, IList<int>>(64);
        }

        public void AddDirect(int sourceTypeHash, int targetTypeHash, object instance)
        {
            var instanceHash = this.idProvider.GetId(instance.GetType());

            if (this.dependencyContainer.ContainsKey(sourceTypeHash))
            {
                var references = this.dependencyContainer[sourceTypeHash];
                if (references.IndexOf(instanceHash) == -1)
                {
                    references.Add(instanceHash);
                }
            }
            else
            {
                this.dependencyContainer.Add(sourceTypeHash, new List<int> { instanceHash });
            }

            if (this.dependencyContainer.ContainsKey(targetTypeHash))
            {
                var references = this.dependencyContainer[targetTypeHash];
                if (references.IndexOf(instanceHash) == -1)
                {
                    references.Add(instanceHash);
                }
            }
            else
            {
                this.dependencyContainer.Add(targetTypeHash, new List<int> { instanceHash });
            }

            if (!this.objectContainer.ContainsKey(instanceHash))
            {
                this.objectContainer.Add(instanceHash, instance);
            }
        }

        public void Add<T>(T instance)
        {
            Add(typeof(T), instance);
        }

        public void Add(Type type, object instance)
        {
            var typeHash = this.idProvider.GetId(type);
            var instanceHash = this.idProvider.GetId(instance.GetType());

            if (this.dependencyContainer.ContainsKey(typeHash))
            {
                var references = this.dependencyContainer[typeHash];
                if (references.IndexOf(instanceHash) == -1)
                {
                    references.Add(instanceHash);
                }
            }
            else
            {
                this.dependencyContainer.Add(typeHash, new List<int> { instanceHash });
            }

            if (this.objectContainer.ContainsKey(instanceHash))
            {
                this.objectContainer[instanceHash] = instance;
            }
            else
            {
                this.objectContainer.Add(instanceHash, instance);
            }
        }

        public void RemoveType(Type type)
        {
            var typeHash = this.idProvider.GetId(type);

            if (this.dependencyContainer.ContainsKey(typeHash))
            {
                var references = this.dependencyContainer[typeHash];
                var length = references.Count;

                for (var i = 0; i < length; i++)
                {
                    var instanceHash = references[i];

                    if (this.objectContainer.ContainsKey(instanceHash))
                    {
                        this.objectContainer.Remove(instanceHash);
                    }
                }

                this.dependencyContainer.Remove(typeHash);
            }
        }

        public void RemoveType<T>()
        {
            RemoveType(typeof(T));
        }

        public void RemoveInstance<T>(T instance)
        {
            var instanceHash = this.idProvider.GetId(instance.GetType());

            if (this.objectContainer.ContainsKey(instanceHash))
            {
                this.objectContainer.Remove(instanceHash);
            }

            foreach (var dependency in this.dependencyContainer)
            {
                var references = dependency.Value;
                var index = references.IndexOf(instanceHash);
                if (index != -1)
                {
                    references.RemoveAt(index);
                }
            }
        }

        public void RemoveInstance(object instance)
        {
            var instanceHash = this.idProvider.GetId(instance.GetType());

            if (this.objectContainer.ContainsKey(instanceHash))
            {
                this.objectContainer.Remove(instanceHash);
            }

            foreach (var dependency in this.dependencyContainer)
            {
                var references = dependency.Value;
                var index = references.IndexOf(instanceHash);
                if (index != -1)
                {
                    references.RemoveAt(index);
                }
            }
        }

        public T First<T>()
        {
            T instance = default;

            var typeHash = this.idProvider.GetId(typeof(T));

            if (this.dependencyContainer.ContainsKey(typeHash))
            {
                var references = this.dependencyContainer[typeHash];
                if (references.Count > 0)
                {
                    var instanceHash = references[0];
                    if (this.objectContainer.ContainsKey(instanceHash))
                    {
                        instance = (T)this.objectContainer[instanceHash];
                    }
                }
            }

            return instance;
        }

        public object First(Type type)
        {
            object instance = default;

            var typeHash = this.idProvider.GetId(type);

            if (this.dependencyContainer.ContainsKey(typeHash))
            {
                var references = this.dependencyContainer[typeHash];
                if (references.Count > 0)
                {
                    var instanceHash = references[0];
                    if (this.objectContainer.ContainsKey(instanceHash))
                    {
                        instance = this.objectContainer[instanceHash];
                    }
                }
            }

            return instance;
        }

        public T[] All<T>()
        {
            T[] instances = default;

            var typeHash = this.idProvider.GetId(typeof(T));

            if (this.dependencyContainer.ContainsKey(typeHash))
            {
                var references = this.dependencyContainer[typeHash];
                var referencesLength = references.Count;

                instances = new T[referencesLength];

                for (var i = 0; i < referencesLength; i++)
                {
                    var referenceHash = references[i];
                    instances[i] = (T)this.objectContainer[referenceHash];
                }
            }

            return instances;
        }

        public object[] All(Type type)
        {
            object[] instances = default;

            var typeHash = this.idProvider.GetId(type);

            if (this.dependencyContainer.ContainsKey(typeHash))
            {
                var references = this.dependencyContainer[typeHash];
                var referencesLength = references.Count;

                instances = new object[referencesLength];

                for (var i = 0; i < referencesLength; i++)
                {
                    var referenceHash = references[i];
                    instances[i] = this.objectContainer[referenceHash];
                }
            }

            return instances;
        }

        public void Dispose()
        {
            this.objectContainer?.Clear();
            this.dependencyContainer?.Clear();
        }
    }
}