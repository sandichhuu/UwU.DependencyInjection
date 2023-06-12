using UnityEngine;

namespace UwU.BezierSolver
{
    public partial class CurveBehaviour : MonoBehaviour, ICurve
    {
        private Float3[] points;
        private CurveNodeBehaviour[] childs;
        private float[] weights;

        private void Start()
        {
            Refresh();
        }

        private void FixedUpdate()
        {
            Refresh();
        }

        public void Refresh()
        {
            if (this.childs == null)
            {
                this.childs = GetComponentsInChildren<CurveNodeBehaviour>();
            }

            if (this.points == null)
            {
                this.points = new Float3[this.childs.Length];
            }

            if (this.weights == null)
            {
                this.weights = new float[this.childs.Length];
            }

            var length = this.points.Length;
            for (var i = 0; i < length; i++)
            {
                this.points[i] = (Float3)this.childs[i].GetPosition();
                this.weights[i] = this.childs[i].weight;
            }
        }

        public Vector3 Solve(float normalizedTime)
        {
            var point = Vector3.zero;

            if (this.points != null)
            {
                //point = Bezier.Solve(normalizedTime, this.points);
                //point = Bezier.SolveHeavy(normalizedTime, this.points);
                point = Bezier.Rational(normalizedTime, this.points, this.weights);
            }

            return point;
        }

        public Vector2 Solve2D(float normalizedTime)
        {
            var point = Vector2.zero;

            if (this.points != null)
            {
                //point = Bezier.Solve(normalizedTime, this.points);
                //point = Bezier.SolveHeavy(normalizedTime, this.points);
                point = Bezier.Rational(normalizedTime, this.points, this.weights);
            }

            return point;
        }

        public void SetPointPosition(int index, Vector3 position)
        {
            this.childs[index].SetPosition(position);
        }

        public Float3 GetPoint(int index)
        {
            return this.points[index];
        }

        public float GetWeight(int index)
        {
            return this.weights[index];
        }

        public int GetLength()
        {
            return this.points.Length;
        }

        public Float3 GetFarNodePoint()
        {
            var nodePoint = this.points[0];
            var length = this.points.Length;

            for (var i = 1; i < length; i++)
            {
                if (nodePoint.z < this.points[i].z)
                {
                    nodePoint = this.points[i];
                }
            }

            return nodePoint;
        }

        public Float3 GetNearNodePoint()
        {
            var nodePoint = this.points[0];
            var length = this.points.Length;

            for (var i = 1; i < length; i++)
            {
                if (nodePoint.z > this.points[i].z)
                {
                    nodePoint = this.points[i];
                }
            }

            return nodePoint;
        }
    }
}