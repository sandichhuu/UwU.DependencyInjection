namespace UwU.ObserverSystem
{
    public interface IReactOn<Arg> : ISubscriber
    {
        void OnNotify(Arg arg);
    }

    public interface ISubscriber
    {
    }
}