using UwU.DI;

public static class Extension
{
    public static void Inject(this object self)
    {
        DIContext.SelfInstance.injector.Inject(self);
    }
}