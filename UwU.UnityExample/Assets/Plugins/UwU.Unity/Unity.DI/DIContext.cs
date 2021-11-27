using UnityEngine;

namespace UwU.Unity.DI
{
    using UwU.DI;
    using UwU.DI.Binding;
    using UwU.DI.Container;
    using UwU.DI.Injection;
    using UwU.Unity.Log;

    public abstract class DIContext : MonoBehaviour
    {
        private UnityLogger unityLogger;
        private UwU.DI.DIContext context;

        [Inject] public readonly IDependencyContainer container;
        [Inject] public readonly IBinder binder;
        [Inject] public readonly IInjector injector;

        [SerializeField] private bool useMultiThreading = false;

        private void Awake()
        {
            this.unityLogger = new UnityLogger();
            this.context = new UwU.DI.DIContext(this.unityLogger, this.useMultiThreading);

            this.Inject();

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