using System;
using System.Collections.Generic;

namespace UwU.DI.Collection
{
    public class SingleThreadReadOnlyCollection<T> : IReadOnlyCollection<T>
    {
        private readonly List<T> items;

        public SingleThreadReadOnlyCollection(int capacity)
        {
            this.items = new List<T>(capacity);
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
                    iterator?.Invoke(this.items[i]);
                }
            }
        }

        public void Dispose()
        {
            this.items?.Clear();
        }
    }
}