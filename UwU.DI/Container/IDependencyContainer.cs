using System;

namespace UwU.DI.Container
{
    public interface IDependencyContainer
    {
        void AddDirect(long sourceTypeHash, long targetTypeHash, object instance);

        void Add<T>(T instance);

        void Add(Type type, object instance);

        void RemoveType(Type type);

        void RemoveType<T>();

        void RemoveInstance<T>(T instance);

        void RemoveInstance(object instance);

        T First<T>();

        object First(Type type);

        T[] All<T>();

        object[] All(Type type);
    }
}