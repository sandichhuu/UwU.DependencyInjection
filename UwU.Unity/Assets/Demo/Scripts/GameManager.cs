using UnityEngine;
using UwU.DI;
using UwU.IFS;
using UwU.Unity.DI;

namespace UwU.Demo
{
    public class GameManager : UnityContext
    {
        [Inject] private readonly ShapeManager shapeManager;

        private ILoop shapeLooper;

        public override void Setup()
        {
            this.binder.BindComponentCommand<ShapeManager>();   // Find component ShapeManager from hierarchy and bind.
            this.binder.ExecuteBindingCommand();                // Do all command above this line.

            this.Inject();                                      // Inject ShapeManager to GameManager (this instance).
        }

        public override void Initialize()
        {
            this.shapeManager.Inject();                         // Make sure shape manager is provided all references.
            this.shapeManager.Setup();                          // This function is replaced Start function.
            this.shapeLooper = this.shapeManager;
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            this.shapeLooper.Loop(deltaTime);
        }
    }

    public static class GameManagerExtension
    {
        private static GameManager Instance;

        public static void Inject(this object obj)
        {
            if (Instance == null)
            {
                Instance = UnityEngine.Object.FindObjectOfType<GameManager>();
            }

            Instance.injector.Inject(obj);
            obj.SolveSceneComponent();
        }
    }
}