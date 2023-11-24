using Collections.Pooled;
using System;

namespace UwU.DI.Container
{
    public class DefaultDependencyContainer : IDependencyContainer, IDisposable
    {
        private readonly PooledDictionary<string, object> objectContainer;
        private readonly PooledDictionary<long, PooledList<string>> dependencyContainer;

        public DefaultDependencyContainer()
        {
            this.objectContainer = new PooledDictionary<string, object>(64);
            this.dependencyContainer = new PooledDictionary<long, PooledList<string>>(64);
        }

        public void AddDirect(long sourceTypeHash, long targetTypeHash, object instance)
        {
            var instanceHash = instance.GetId();

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
                this.dependencyContainer.Add(sourceTypeHash, new PooledList<string> { instanceHash });
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
                this.dependencyContainer.Add(targetTypeHash, new PooledList<string> { instanceHash });
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
            var typeHash = type.GetId();
            var instanceHash = instance.GetId();

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
                this.dependencyContainer.Add(typeHash, new PooledList<string> { instanceHash });
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
            var typeHash = type.GetId();

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
            var instanceHash = instance.GetId();

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
            var instanceHash = instance.GetId();

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
            T instance = default(T);

            var typeHash = typeof(T).GetId();

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
            object instance = null;

            var typeHash = type.GetId();

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
            T[] instances = default(T[]);
            var typeHash = typeof(T).GetId();

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
            object[] instances = default(object[]);
            var typeHash = type.GetId();

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
            if (this.objectContainer != null)
            {
                this.objectContainer.Clear();
            }

            if (this.dependencyContainer != null)
            {
                this.dependencyContainer.Clear();
            }
        }
    }
}