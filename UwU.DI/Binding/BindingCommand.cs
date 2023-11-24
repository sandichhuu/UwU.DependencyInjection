namespace UwU.DI.Binding
{
    using UwU.DI.GC;

    public struct BindingCommand
    {
        public long sourceTypeHash;
        public long targetTypeHash;
        public ObjectHandler instaceHandle;
    }
}