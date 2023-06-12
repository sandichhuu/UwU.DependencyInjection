using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace UwU.DI.Collection
{
    public class MultiThreadReadOnlyCollection<T> : IReadOnlyCollection<T>
    {
        private readonly BlockingCollection<T> items;

        public MultiThreadReadOnlyCollection(int capacity)
        {
            this.items = new BlockingCollection<T>(capacity);
        }

        public void Add(T item)
        {
            this.items.Add(item);
        }

        /// <summary>
        /// Warining: When call this method, collection cannot add any more item !
        /// </summary>
        /// <param name="iterator"></param>
        public void ForEach(Action<T> iterator)
        {
            this.items.CompleteAdding();

            Parallel.ForEach(this.items, item =>
            {
                iterator.Invoke(item);
            });
        }

        public void Dispose()
        {
            this.items.Dispose();
        }
    }
}