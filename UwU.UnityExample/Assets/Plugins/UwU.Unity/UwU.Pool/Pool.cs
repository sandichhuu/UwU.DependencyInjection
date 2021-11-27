using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

namespace UwU
{
    public class Pool<T> : IPool
    {
        public ItemEvent onNewItemCreated;
        public ItemEvent onItemReturnToPool;
        public ItemEvent onItemRequested;

        public delegate void ItemEvent(T item);

        private Queue<PoolItem> free;
        private Dictionary<int, PoolItem> busy;

        private T sample;

        public void Initialize(T sample, int capacity = 16)
        {
            this.sample = sample;
            this.free = new Queue<PoolItem>(capacity);
            this.busy = new Dictionary<int, PoolItem>(capacity);

            var instances = SpawnInstanceBuck(capacity);
            for (var i = 0; i < capacity; i++)
            {
                this.free.Enqueue(BuildNewPoolItem(instances[i]));
            }
        }

        public T Request()
        {
            PoolItem freeItem = null;

            if (this.free.Count > 0)
                freeItem = this.free.Dequeue();

            if (freeItem == null)
            {
                freeItem = BuildNewPoolItem(SpawnInstance());
                Debug.LogWarning("Warning ! There are no free item, add new item !");
            }

            freeItem.isFree = false;

            this.busy.Add(freeItem.uid, freeItem);

            var requestedItem = (T)freeItem.obj;
            this.onItemRequested?.Invoke(requestedItem);
            return requestedItem;
        }

        public void ReturnItem(T item)
        {
            var uid = item.GetHashCode();
            if (this.busy.ContainsKey(uid))
            {
                var poolItem = this.busy[uid];
                poolItem.isFree = true;

                this.busy.Remove(uid);
                this.free.Enqueue(poolItem);

                this.onItemReturnToPool?.Invoke(item);
            }
            else
            {
                Debug.LogWarning($"The item with [uid: {uid}] is not in using, recheck please !");
            }
        }

        private PoolItem BuildNewPoolItem(T obj)
        {
            this.onNewItemCreated?.Invoke(obj);
            return new PoolItem(obj);
        }

        private T[] SpawnInstanceBuck(int quantity)
        {
            var instances = new T[quantity];

            if (this.sample is Component component)
            {
                for (var i = 0; i < quantity; i++)
                    instances[i] = Object.Instantiate(component).GetComponent<T>();
            }
            else
            {
                instances = DeepCloneBuck(this.sample, quantity);
            }

            return instances;
        }

        private T SpawnInstance()
        {
            T instance;

            if (this.sample is Component component)
            {
                instance = Object.Instantiate(component).GetComponent<T>();
            }
            else
            {
                instance = DeepClone(this.sample);
            }

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

        private T[] DeepCloneBuck(T obj, int quantity)
        {
            var instances = new T[quantity];

            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);

                for (var i = 0; i < quantity; i++)
                {
                    stream.Position = 0;
                    instances[i] = (T)formatter.Deserialize(stream);
                }
            }

            return instances;
        }

        object IPool.RequestUnsafe()
        {
            return Request();
        }

        public void ReturnItemUnsafe(object objectToReturn)
        {
            ReturnItem((T)objectToReturn);
        }
    }

    public class PoolItem
    {
        public readonly int uid;
        public readonly object obj;
        public bool isFree;

        public PoolItem(object obj)
        {
            this.isFree = true;
            this.obj = obj;
            this.uid = obj.GetHashCode();
        }
    }

    public interface IPool
    {
        object RequestUnsafe();

        void ReturnItemUnsafe(object objectToReturn);
    }
}