using System;
using System.Linq;
using System.Reflection;

namespace UwU.UnityBridge
{
    public class GameObject
    {
        private static bool IsTypeInitialized;

        private static MethodInfo FindMethod;
        private static MethodInfo GetComponentMethod;

        public static void Initialize(Type unityEngineType)
        {
            FindMethod = unityEngineType.GetMethod("Find", BindingFlags.Static | BindingFlags.Public);

            GetComponentMethod = unityEngineType
                .GetMethods()
                .Single(m => m.Name == "GetComponent" && m.IsGenericMethod);

            IsTypeInitialized = true;
        }

        public readonly object gameObject;

        public GameObject(object gameObject)
        {
            this.gameObject = gameObject;
        }

        public static GameObject Find(string name)
        {
            if (!IsTypeInitialized)
                throw new Exception("UnityBridge.GameObject not initialized !");

            return new GameObject(FindMethod.Invoke(null, new[] { name }));
        }

        public T GetComponent<T>()
        {
            if (!IsTypeInitialized)
                throw new Exception("UnityBridge.GameObject not initialized !");

            var genericMethod = GetComponentMethod.MakeGenericMethod(typeof(T));
            return (T)genericMethod.Invoke(this.gameObject, null);
        }

        public static bool IsReady()
        {
            return IsTypeInitialized;
        }
    }
}