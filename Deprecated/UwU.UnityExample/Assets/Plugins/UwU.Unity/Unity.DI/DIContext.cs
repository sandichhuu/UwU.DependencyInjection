using UnityEngine;

namespace UwU.Unity.DI
{
    using UwU.DI.Binding;
    using UwU.DI.Container;
    using UwU.DI.Injection;
    using UwU.Unity.Logger;

    public abstract class DIContext : MonoBehaviour
    {
        private UnityLogger unityLogger;
        private UwU.DI.DIContext context;

        public IDependencyContainer container { get; private set; }
        public IBinder binder { get; private set; }
        public IInjector injector { get; private set; }

        private void Awake()
        {
            UnityBridge.GameObject.Initialize(typeof(GameObject));

            this.unityLogger = new UnityLogger();
            this.context = new UwU.DI.DIContext(this.unityLogger);

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