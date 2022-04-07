using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cephei
{
    public static class VectorExtantion
    {
        public static Vector3 ProjectOnPlane(this Vector3 origin, Vector3 planeNormal)
        {
            return Vector3.ProjectOnPlane(origin, planeNormal);
        }

        public static Vector3 ZeroY(this Vector3 origin)
        {
            origin.y = 0;
            return origin;
        }

        public static float SqrDistance(this Vector3 vector, Vector3 other)
        {
            return (vector - other).sqrMagnitude;
        }

        public static float MaxNumber(this Vector3 vector)
        {
            return Mathf.Max(vector.x, vector.y, vector.z);
        }

        public static float GetRandomValue(this Vector2 vector)
        {
            return Random.Range(Mathf.Min(vector.x, vector.y), Mathf.Max(vector.x, vector.y));
        }

        public static Quaternion GetRotation(this Vector3 vector) => Quaternion.LookRotation(vector);

        public static Vector3 GetRandomPositionInBox(Vector3 origin, Vector3 volume)
        {
            System.Random random = new System.Random();

            Vector3 rightRandom = Vector3.right * volume.x * random.Sign() * random.Value();
            Vector3 upRandom = Vector3.up * volume.y * random.Sign() * random.Value();
            Vector3 forwardRandom = Vector3.forward * volume.z * random.Sign() * random.Value();

            return origin + rightRandom + upRandom + forwardRandom;
        }

        public static Vector3 To(this Vector3 vector, Vector3 to) => to - vector;

        public static Vector3 From(this Vector3 vector, Vector3 from) => vector - from;

        public static Vector3 TurnOnSelfRight(this Vector3 vector, float angle)
        {
            Vector3 vectorsRight = Vector3.Cross(vector, Vector3.up);
            return Quaternion.AngleAxis(angle, vectorsRight) * vector;
        }

        public static Vector3 GetRight(this Vector3 vector) => Vector3.Cross(vector, Vector3.up).normalized;

        public static Vector3 GetUp(this Vector3 vector) => Vector3.Cross(vector.GetRight(), vector); // No Test
    }
}