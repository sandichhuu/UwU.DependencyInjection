using UnityEngine;

namespace UwU.Unity.Logger
{
    public class UnityLogger : UwU.Logger.ILogger
    {
        public void Error(object message)
        {
#if UNITY_EDITOR
            Debug.LogWarning(message);
#endif
        }

        public void Trace(object message)
        {
#if UNITY_EDITOR
            Debug.Log($"<color=cyan>{message}</color>");
#endif
        }

        public void Warn(object message)
        {
#if UNITY_EDITOR
            Debug.LogWarning(message);
#endif
        }
    }
}