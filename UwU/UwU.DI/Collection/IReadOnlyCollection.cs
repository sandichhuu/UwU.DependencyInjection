using System;

namespace UwU.DI.Collection
{
    public interface IReadOnlyCollection<T> : IDisposable
    {
        void Add(T item);

        void ForEach(Action<T> iterator);
    }
}