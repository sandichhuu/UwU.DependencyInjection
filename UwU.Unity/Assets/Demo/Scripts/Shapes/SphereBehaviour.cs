using UnityEngine;

namespace UwU.Demo.Shapes
{
    public class SphereBehaviour : MonoBehaviour, ILoop
    {
        private Vector3 startPosition;

        public void Setup()
        {
            this.startPosition = this.transform.position;
        }

        public void Loop(float dt)
        {
            var movementX = Mathf.Sin(Time.time);
            this.transform.position = this.startPosition + new Vector3(movementX, 0, 0);
        }
    }
}