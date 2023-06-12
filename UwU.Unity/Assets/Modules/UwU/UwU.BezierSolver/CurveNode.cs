using UnityEngine;

namespace UwU.BezierSolver
{
    public struct CurveNode
    {
        public readonly float weight;
        public Float3 position { get; private set; }

        public CurveNode(Vector3 position, float weight)
        {
            this.weight = weight;
            this.position = (Float3)position;
        }

        public void SetPosition(Vector3 position)
        {
            this.position = (Float3)position;
        }
    }
}