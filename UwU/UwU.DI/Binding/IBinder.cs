namespace UwU.DI.Binding
{
    public interface IBinder
    {
        void BindRelevantsTypeCommand(object instance, bool ignoreSystemType);

        void BindCommand<SourceType, TargetType>(TargetType instance);

        void Unbind<T>();

        void Unbind<T>(T obj);

        void ExecuteBindingCommand();
    }
}