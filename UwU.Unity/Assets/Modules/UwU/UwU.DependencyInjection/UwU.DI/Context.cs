using System;

namespace UwU.DI
{
    using UwU.DI.Binding;
    using UwU.DI.Container;
    using UwU.DI.Injection;
    using UwU.Logger;
    using UwU.TypeId;

    public sealed class Context : IDisposable
    {
        public readonly IDependencyContainer container;
        public readonly IBinder binder;
        public readonly IInjector injector;
        public readonly IdProvider idProvider;
        public readonly ILogger logger;

        public Context(ILogger logger)
        {
            this.logger = logger;
            this.idProvider = new IdProvider();
            this.container = new IDContainer(this.idProvider);
            this.binder = new Binder(this.idProvider, this.container, this.logger, false);
            this.injector = new Injector(this.container, this.logger);

            var ignoreNamespace = new[]
            {
                "System", "UnityEngine"
            };

            this.binder.BindRelevantsTypeCommand(this.container, ignoreNamespace);
            this.binder.BindRelevantsTypeCommand(this.binder, ignoreNamespace);
            this.binder.BindRelevantsTypeCommand(this.injector, ignoreNamespace);

            this.binder.ExecuteBindingCommand();
        }

        public void Dispose()
        {
            if (this.container is IDisposable containerDisposable)
            {
                containerDisposable.Dispose();
            }

            if (this.binder is IDisposable binderDisposable)
            {
                binderDisposable.Dispose();
            }

            if (this.injector is IDisposable injectorDisposable)
            {
                injectorDisposable.Dispose();
            }
        }
    }
}