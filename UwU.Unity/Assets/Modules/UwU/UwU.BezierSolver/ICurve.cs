using UnityEngine;

namespace UwU.BezierSolver
{
    public interface ICurve
    {
        void SetPointPosition(int index, Vector3 position);
        Float3 GetPoint(int index);
        float GetWeight(int index);
        int GetLength();
        Float3 GetFarNodePoint();
        Float3 GetNearNodePoint();
        void Refresh();
        Vector3 Solve(float normalizedTime);
    }
}