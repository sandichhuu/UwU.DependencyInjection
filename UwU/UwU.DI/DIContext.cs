using System;

namespace UwU.DI
{
    using UwU.Log;
    using UwU.DI.Binding;
    using UwU.DI.Container;
    using UwU.DI.Injection;

    public sealed class DIContext : IDisposable
    {
        public static DIContext SelfInstance;

        public readonly IDependencyContainer container;
        public readonly IBinder binder;
        public readonly IInjector injector;

        public readonly ILogger logger;

        public DIContext(ILogger logger, bool useMultiThread)
        {
            this.logger = logger;

            this.container = new HashContainer();
            this.binder = new Binder(this.container, this.logger, useMultiThread);
            this.injector = new Injector(this.container, this.logger);

            if (SelfInstance == null)
                SelfInstance = this;

            this.binder.BindRelevantsTypeCommand(this.container, true);
            this.binder.BindRelevantsTypeCommand(this.binder, true);
            this.binder.BindRelevantsTypeCommand(this.injector, true);

            this.binder.ExecuteBindingCommand();
        }

        public void Dispose()
        {
            if (this.container is IDisposable containerDisposable)
                containerDisposable.Dispose();

            if (this.binder is IDisposable binderDisposable)
                binderDisposable.Dispose();

            if (this.injector is IDisposable injectorDisposable)
                injectorDisposable.Dispose();
        }
    }
}