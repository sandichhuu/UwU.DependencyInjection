using UwU.IFS;
using UwU.Unity.DI;

namespace UwU.Template
{
    public class GameManager : UnityContext
    {
        public override void Setup()
        {
        }

        public override void Initialize()
        {
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