using UnityEngine;

namespace UwU.Example
{
    public class CircleBehaviour : MonoBehaviour, IAwake, IStart, IUpdate
    {
        void IAwake.Awake()
        {
            Debug.Log($"{this.gameObject.name}.Awake");
        }

        void IStart.Start()
        {
            Debug.Log($"{this.gameObject.name}.Start");
        }

        void IUpdate.Update(float deltaTime)
        {
            this.transform.RotateAround(Vector3.zero, Vector3.forward, deltaTime * 60);
        }
    }
}