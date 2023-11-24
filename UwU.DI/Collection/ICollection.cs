using System;

namespace UwU.DI.Collection
{
    public interface ICollection<T> : IDisposable
    {
        void Add(T item);

        void ForEach(Action<T> iterator);
    }
}