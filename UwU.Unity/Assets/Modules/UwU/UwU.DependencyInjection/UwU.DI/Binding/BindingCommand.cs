namespace UwU.DI.Binding
{
    using UwU.DI.GC;

    public struct BindingCommand
    {
        public int sourceTypeHash;
        public int targetTypeHash;
        public ObjectHandler instaceHandle;
    }
}