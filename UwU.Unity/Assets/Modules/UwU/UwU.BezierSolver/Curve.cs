using UnityEngine;

namespace UwU.BezierSolver
{
    public partial class Curve : ICurve
    {
        private CurveNode[] nodes;
        private Float3[] points;
        private float[] weights;

        public Curve(CurveNode[] nodes)
        {
            ApplyNodes(nodes);
        }

        public void UpdatePoints()
        {
            var length = this.points.Length;
            for (var i = 0; i < length; i++)
            {
                this.points[i] = this.nodes[i].position;
                this.weights[i] = this.nodes[i].weight;
            }
        }

        public void ApplyNodes(CurveNode[] nodes)
        {
            this.nodes = nodes;
            this.points = new Float3[this.nodes.Length];
            this.weights = new float[this.nodes.Length];

            UpdatePoints();
        }

        public void Refresh()
        {
            this.points = new Float3[this.nodes.Length];
            this.weights = new float[this.nodes.Length];

            UpdatePoints();
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

        public void SetPointPosition(int index, Vector3 position)
        {
            this.nodes[index].SetPosition(position);
        }
    }
}