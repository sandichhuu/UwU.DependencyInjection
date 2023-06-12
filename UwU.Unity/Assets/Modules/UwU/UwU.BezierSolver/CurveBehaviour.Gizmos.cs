#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace UwU.BezierSolver
{
    public partial class CurveBehaviour : MonoBehaviour
    {
        [Space]
        [Header("Gizmos")]
        [SerializeField] private bool debug = true;

        [Space]
        [SerializeField, Range(1, 100)] private int detailLevel = 100;
        [SerializeField, Range(.1f, 1f)] private float nodeSize = 1f;
        [SerializeField] private Color nodeColor = Color.cyan;
        [SerializeField] private Color pathColor = Color.cyan;
        [SerializeField] private Color farPathColor = Color.red;
        [SerializeField] private Color nodeConnectionColor = Color.white;

        private void DrawPath()
        {
            Gizmos.color = this.pathColor;

            var point0 = Solve(0);
            var step = 1f / this.detailLevel;

            var farNode = GetFarNodePoint();
            var nearNode = GetNearNodePoint();
            var headingZ = (farNode - nearNode).z;

            for (var i = 1; i < this.detailLevel - 1; i++)
            {
                var point1 = Solve(i * step);
                Gizmos.color = Color.Lerp(this.pathColor, this.farPathColor, point1.z / headingZ);
                Gizmos.DrawLine(point0, point1);
                point0 = point1;
            }
            Gizmos.DrawLine(point0, Solve(1));
        }

        private void DrawNode()
        {
            for (var i = 0; i < this.points.Length; i++)
            {
                Gizmos.DrawWireCube((Vector3)this.points[i], Vector3.one * this.nodeSize);
            }
        }

        private void DrawNodeConnection()
        {
            for (var i = 0; i < this.points.Length - 1; i++)
            {
                Handles.DrawDottedLine(this.points[i], this.points[i + 1], 5f);
            }
        }

        private void OnDrawGizmos()
        {
            if (this.debug == false)
            {
                return;
            }

            if (Camera.current == Camera.main || Camera.current == SceneView.lastActiveSceneView.camera)
            {
                RefreshOnEditor();

                var originalGizmosColor = Gizmos.color;
                var originalHandlesColor = Handles.color;

                DrawPath();
                Gizmos.color = this.nodeColor;
                DrawNode();
                Handles.color = this.nodeConnectionColor;
                DrawNodeConnection();

                Gizmos.color = originalGizmosColor;
                Handles.color = originalHandlesColor;
            }
        }

        public void RefreshOnEditor()
        {
            this.childs = GetComponentsInChildren<CurveNodeBehaviour>();
            this.points = new Float3[this.childs.Length];
            this.weights = new float[this.childs.Length];
            Refresh();
        }
    }
}
#endif