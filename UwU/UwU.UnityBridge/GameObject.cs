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
        private static MethodInfo FindObjectOfTypeMethod;

        public static void Initialize(Type unityEngineType)
        {
            FindMethod = unityEngineType.GetMethod("Find", BindingFlags.Static | BindingFlags.Public);

            GetComponentMethod = unityEngineType
                .GetMethods()
                .Single(m => m.Name == "GetComponent" && m.IsGenericMethod);

            FindObjectOfTypeMethod = unityEngineType.BaseType
                .GetMethods()
                .Single(m => m.Name == "FindObjectOfType" && m.IsGenericMethod && m.GetParameters().Length > 0);

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

            var gameObject = FindMethod.Invoke(null, new[] { name });

            if (gameObject == null)
                throw new Exception($"GameObject [{name}] not found on scene !");

            return new GameObject(gameObject);
        }

        public T GetComponent<T>()
        {
            if (!IsTypeInitialized)
                throw new Exception("UnityBridge.GameObject not initialized !");

            var genericMethod = GetComponentMethod.MakeGenericMethod(typeof(T));
            var component = genericMethod.Invoke(this.gameObject, null);

            if (component == null)
                throw new Exception($"Component [{typeof(T).Name}] not found !");

            return (T)component;
        }

        public static T FindObjectOfType<T>()
        {
            if (!IsTypeInitialized)
                throw new Exception("UnityBridge.GameObject not initialized !");

            var genericMethod = FindObjectOfTypeMethod.MakeGenericMethod(typeof(T));
            var component = genericMethod.Invoke(null, new object[] { true });

            if (component == null)
                throw new Exception($"Component [{typeof(T).Name}] not found !");

            return (T)component;
        }

        public static bool IsReady()
        {
            return IsTypeInitialized;
        }
    }
}