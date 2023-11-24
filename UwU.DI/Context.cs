using System;

namespace UwU.DI
{
    using UwU.DI.Binding;
    using UwU.DI.Container;
    using UwU.DI.Injection;
    using UwU.Core;

    public sealed class Context : IDisposable
    {
        public readonly IDependencyContainer container;
        public readonly IBinder binder;
        public readonly IInjector injector;

        public readonly ILogger logger;

        public Context(ILogger logger)
        {
            this.logger = logger;

            container = new DefaultDependencyContainer();
            binder = new Binder(container, this.logger);
            injector = new Injector(container, this.logger);

            var ignoreNamespace = new[]
            {
                "System", "UnityEngine"
            };

            binder.BindRelevantsTypeCommand(container, ignoreNamespace);
            binder.BindRelevantsTypeCommand(binder, ignoreNamespace);
            binder.BindRelevantsTypeCommand(injector, ignoreNamespace);

            binder.ExecuteBindingCommand();
        }

        public void Dispose()
        {
            if (container is IDisposable)
            {
                (container as IDisposable).Dispose();
            }

            if (binder is IDisposable)
            {
                (binder as IDisposable).Dispose();
            }

            if (injector is IDisposable)
            {
                (injector as IDisposable).Dispose();
            }
        }
    }
}