using Collections.Pooled;
using System;

namespace UwU.DI.Collection
{
    public class ReadOnlyCollection<T> : ICollection<T>
    {
        private readonly PooledList<T> items;

        public ReadOnlyCollection(int capacity)
        {
            this.items = new PooledList<T>(capacity);
        }

        public void Add(T item)
        {
            this.items.Add(item);
        }

        public void ForEach(Action<T> iterator)
        {
            lock (this.items)
            {
                var length = this.items.Count;
                for (var i = 0; i < length; i++)
                {
                    if (iterator != null)
                    {
                        iterator.Invoke(this.items[i]);
                    }
                }
            }
        }

        public void Dispose()
        {
            if (this.items != null)
            {
                this.items.Clear();
            }
        }
    }
}