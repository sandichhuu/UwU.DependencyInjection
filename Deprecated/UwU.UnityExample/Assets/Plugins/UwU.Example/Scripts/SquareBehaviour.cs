using UnityEngine;

namespace UwU.Example
{
    public class SquareBehaviour : MonoBehaviour, IAwake, IStart, IUpdate
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
            var velocity = 5;
            this.transform.Rotate(Vector3.forward, velocity * deltaTime);
        }
    }
}