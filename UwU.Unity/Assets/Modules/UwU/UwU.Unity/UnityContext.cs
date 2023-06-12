using UnityEngine;

namespace UwU.Unity.DI
{
    using UwU.DI;
    using UwU.DI.Binding;
    using UwU.DI.Container;
    using UwU.DI.Injection;

    public abstract class UnityContext : MonoBehaviour
    {
        private Logger.UnityLogger unityLogger;
        private Context context;

        public IDependencyContainer container { get; private set; }
        public IBinder binder { get; private set; }
        public IInjector injector { get; private set; }

        private void Awake()
        {
            this.unityLogger = new Logger.UnityLogger();
            this.context = new Context(this.unityLogger);
            //this.context = new Context(null);

            this.container = this.context.container;
            this.binder = this.context.binder;
            this.injector = this.context.injector;

            Setup();
        }

        private void Start()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            this.context.Dispose();
        }

        public abstract void Setup();

        public abstract void Initialize();
    }
}