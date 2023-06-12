using UnityEngine;

namespace UwU.BezierSolver
{
    public class CurveNodeBehaviour : MonoBehaviour
    {
        [field: SerializeField]
        public float weight { get; private set; } = 1f;

        public Vector3 GetPosition()
        {
            return this.transform.position;
        }

        public Float3 GetPoint()
        {
            return new Float3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        }

        public void SetPosition(Vector3 position)
        {
            this.transform.position = position;
        }
    }
}