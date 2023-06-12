using System;
using System.Collections.Generic;
using UnityEngine;

namespace UwU.BezierSolver
{
    public static class Bezier
    {
        private static readonly Dictionary<int, Func<float, Float3[], Float3>> Solver = new()
        {
            {0, (t, arr) => Float3.Zero },
            {1, (t, arr) => arr[0] },
            {2, (t,arr) => Linear(t, arr[0], arr[1]) },
            {3, (t,arr) => Quadratic(t, arr[0], arr[1], arr[2]) },
            {4, (t,arr) => Cubic(t, arr[0], arr[1], arr[2], arr[3]) },
            {-1, (t, arr) => SolveHeavy(t, arr) }
        };

        public static Float3 Linear(float normalizedTime, Float3 point0, Float3 point1)
        {
            return point0 + (normalizedTime * (point1 - point0));
        }

        public static Float3 Quadratic(float normalizedTime, Float3 point0, Float3 point1, Float3 point2)
        {
            return point1 + (Pow2(1 - normalizedTime) * (point0 - point1)) + (Pow2(normalizedTime) * (point2 - point1));
        }

        public static Float3 Cubic(float normalizedTime, Float3 point0, Float3 point1, Float3 point2, Float3 point3)
        {
            return (Pow3(1 - normalizedTime) * point0)
                + (3 * Pow2(1 - normalizedTime) * normalizedTime * point1)
                + (3 * (1 - normalizedTime) * Pow2(normalizedTime) * point2)
                + (Pow3(normalizedTime) * point3);
        }

        public static Float3 Rational(float normalizedTime, Float3[] points, float[] weights)
        {
            if (normalizedTime == 0)
            {
                return points[0];
            }

            var length = points.Length;

            if (normalizedTime == 1)
            {
                return points[length - 1];
            }

            var numerator = new Float3(0, 0, 0);
            var denominator = 0f;

            for (var i = 0; i < length; i++)
            {
                var weight = weights[i];
                var point = points[i];
                var temp = BernsteinBasisPolynomials(i + 1, length, normalizedTime) * weight;

                numerator += temp * point;
                denominator += temp;
            }

            Float3 result;
            if (numerator.IsValidPosition() == false || denominator == 0)
            {
                result = points[0];
            }
            else
            {
                result = numerator / denominator;
            }

            return result;
        }

        public static Float3 Solve(float normalizedTime, params Float3[] points)
        {
            if (normalizedTime == 0)
            {
                return points[0];
            }

            var length = points.Length;

            if (normalizedTime == 1)
            {
                return points[length - 1];
            }

            var solver = length <= 4 ? Solver[length] : Solver[-1];

            return solver.Invoke(normalizedTime, points);
        }

        public static Float3 SolveHeavy(float normalizedTime, params Float3[] points)
        {
            if (normalizedTime == 0)
            {
                return points[0];
            }

            var length = points.Length;

            if (normalizedTime == 1)
            {
                return points[length - 1];
            }

            Float3[] GetLinear(Float3[] points)
            {
                var length = points.Length;
                var linears = new Float3[length - 1];

                for (var i = 0; i < length - 1; i++)
                {
                    linears[i] = Linear(normalizedTime, points[i], points[i + 1]);
                }

                return linears;
            }

            var linears = GetLinear(points);

            while (linears.Length != 1)
            {
                var tempLinear = GetLinear(linears);
                linears = tempLinear;
            }

            return linears[0];
        }

        private static float Pow2(float x)
        {
            return x * x;
        }

        private static float Pow3(float x)
        {
            return x * x * x;
        }

        private static float PowN(float x, int expo)
        {
            if (expo == 0)
            {
                return 1;
            }

            var temp = PowN(x, expo / 2);

            if (expo % 2 == 0)
            {
                return temp * temp;
            }
            else
            {
                if (expo > 0)
                {
                    return temp * temp * x;
                }
                else
                {
                    return temp * temp / x;
                }
            }
        }

        private static float BernsteinBasisPolynomials(int i, int n, float t)
        {
            return (n * 1.0f / i) * PowN(t, i) * PowN(1 - t, n - i);
        }
    }

    public struct Float2
    {
        public readonly float x;
        public readonly float y;

        public Float2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static readonly Float2 Zero = new(0, 0);
        public static readonly Float2 One = new(1, 1);
        public static readonly Float2 NaN = new(float.NaN, float.NaN);
        public static readonly Float2 Epsilon = new(float.Epsilon, float.Epsilon);
        public static readonly Float2 NegativeInfinity = new(float.NegativeInfinity, float.NegativeInfinity);
        public static readonly Float2 PositiveInfinity = new(float.PositiveInfinity, float.PositiveInfinity);
        public static readonly Float2 MinValue = new(float.MinValue, float.MinValue);
        public static readonly Float2 MaxValue = new(float.MaxValue, float.MaxValue);

        public static Float2 operator *(float f, Float2 a)
        {
            return new Float2(a.x * f, a.y * f);
        }

        public static Float2 operator /(float f, Float2 a)
        {
            return new Float2(a.x / f, a.y / f);
        }

        public static Float2 operator *(Float2 a, float f)
        {
            return new Float2(a.x * f, a.y * f);
        }

        public static Float2 operator /(Float2 a, float f)
        {
            return new Float2(a.x / f, a.y / f);
        }

        public static Float2 operator +(Float2 a, Float2 b)
        {
            return new Float2(a.x + b.x, a.y + b.y);
        }

        public static Float2 operator -(Float2 a, Float2 b)
        {
            return new Float2(a.x - b.x, a.y - b.y);
        }

        public static explicit operator Float2(Vector2 vector2) => new(vector2.x, vector2.y);
        public static explicit operator Float2(Vector3 vector3) => new(vector3.x, vector3.y);
        public static explicit operator Float2(Float3 vector3) => new(vector3.x, vector3.y);

        public static implicit operator Vector2(Float2 float2) => new(float2.x, float2.y);
        public static implicit operator Vector3(Float2 float2) => new(float2.x, float2.y, 0);

        public override string ToString()
        {
            return $"{base.ToString()}({this.x}, {this.y})";
        }
    }

    public struct Float3
    {
        public readonly float x;
        public readonly float y;
        public readonly float z;

        public Float3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static readonly Float3 Zero = new(0, 0, 0);
        public static readonly Float3 One = new(1, 1, 1);
        public static readonly Float3 NaN = new(float.NaN, float.NaN, float.NaN);
        public static readonly Float3 Epsilon = new(float.Epsilon, float.Epsilon, float.Epsilon);
        public static readonly Float3 NegativeInfinity = new(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
        public static readonly Float3 PositiveInfinity = new(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
        public static readonly Float3 MinValue = new(float.MinValue, float.MinValue, float.MinValue);
        public static readonly Float3 MaxValue = new(float.MaxValue, float.MaxValue, float.MaxValue);

        public bool IsValidPosition()
        {
            return this != NaN &&
                this != Epsilon &&
                this != NegativeInfinity &&
                this != PositiveInfinity;
        }

        public static bool operator ==(Float3 a, Float3 b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }

        public static bool operator !=(Float3 a, Float3 b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z;
        }

        public static Float3 operator *(float f, Float3 a)
        {
            return new Float3(a.x * f, a.y * f, a.z * f);
        }

        public static Float3 operator /(float f, Float3 a)
        {
            return new Float3(a.x / f, a.y / f, a.z / f);
        }

        public static Float3 operator *(Float3 a, float f)
        {
            return new Float3(a.x * f, a.y * f, a.z * f);
        }

        public static Float3 operator /(Float3 a, float f)
        {
            return new Float3(a.x / f, a.y / f, a.z / f);
        }

        public static Float3 operator +(Float3 a, Float3 b)
        {
            return new Float3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Float3 operator -(Float3 a, Float3 b)
        {
            return new Float3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static explicit operator Float3(Vector2 vector2) => new(vector2.x, vector2.y, 0);
        public static explicit operator Float3(Vector3 vector3) => new(vector3.x, vector3.y, vector3.z);
        public static explicit operator Float3(Float2 float2) => new(float2.x, float2.y, 0);

        public static implicit operator Vector3(Float3 float3) => new(float3.x, float3.y, float3.z);
        public static implicit operator Vector2(Float3 float3) => new(float3.x, float3.y);

        public override string ToString()
        {
            return $"{base.ToString()}({this.x}, {this.y}, {this.z})";
        }

        public override bool Equals(object obj)
        {
            return obj is Float3 f &&
                   this.x == f.x &&
                   this.y == f.y &&
                   this.z == f.z;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}