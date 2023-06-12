using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;
using Object = UnityEngine.Object;

namespace UwU
{
    public class Pool<T>
    {
        public StartEvent onNewItemCreated;
        public ItemEvent onItemRequested;
        public ItemEvent onItemReturned;

        public delegate void StartEvent(Pool<T> fromPool, T item);
        public delegate void ItemEvent(T item);

        private Queue<T> queue;
        private T sample;

        public void Initialize(T sample, int capacity = 16)
        {
            this.sample = sample;
            this.queue = new Queue<T>(capacity);

            var instances = Create(capacity);
            for (var i = 0; i < capacity; i++)
            {
                this.queue.Enqueue(instances[i]);
            }
        }

        public T Request()
        {
            T result;

            if (this.queue.Count > 0)
            {
                result = this.queue.Dequeue();
            }
            else
            {
                result = Create();
            }

            this.onItemRequested?.Invoke(result);
            return result;
        }

        public void ReturnItem(T item)
        {
#if UNITY_EDITOR
            if (this.queue.Contains(item))
            {
                Debug.LogError("Item is already in pool ! Cannot return item.");
            }
#endif
            this.queue.Enqueue(item);
            this.onItemReturned?.Invoke(item);
        }

        private T[] Create(int quantity)
        {
            var instances = new T[quantity];

            if (this.sample is Component)
            {
                for (var i = 0; i < quantity; i++)
                {
                    instances[i] = Create();
                }
            }
            else
            {
                instances = CreateBuck(this.sample, quantity);
            }

            return instances;
        }

        private T Create()
        {
            T instance;

            if (this.sample is Component component)
            {
                instance = Object.Instantiate(component.gameObject).GetComponent<T>();
            }
            else
            {
                instance = DeepClone(this.sample);
            }

            this.onNewItemCreated?.Invoke(this, instance);
            return instance;
        }

        private T DeepClone(T obj)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Position = 0;

                return (T)formatter.Deserialize(stream);
            }
        }

        private T[] CreateBuck(T obj, int quantity)
        {
            var instances = new T[quantity];

            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);

                for (var i = 0; i < quantity; i++)
                {
                    stream.Position = 0;
                    var instance = (T)formatter.Deserialize(stream);
                    instances[i] = instance;
                    this.onNewItemCreated?.Invoke(this, instance);
                }
            }

            return instances;
        }
    }
}