using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cephei
{
    public static class TransformExtantions
    {
        public static void SetX(this Transform transform, float x)
        {
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }

        public static void SetY(this Transform transform, float y)
        {
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }

        public static void SetZ(this Transform transform, float z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, z);
        }

        public static Vector3 CustomInverseTransformVector(this Transform transform, Vector3 world)
        {
            return transform.right * world.x + transform.up * world.y + transform.forward * world.z;
        }

        public static void MoveTowards(this Transform transform, Vector3 targetPosion, float delta)
            => transform.position = Vector3.MoveTowards(transform.position, targetPosion, delta);
        
        public static void RotateTowards(this Transform transform, Quaternion targetRotation, float delta)
            => transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, delta);

        public static void MoveForward(this Transform transform, float delta)
            => transform.position = transform.position + transform.forward * delta;

        public static void LookTo(this Transform transform, Vector3 direction) =>
            transform.rotation = Quaternion.LookRotation(direction);
    }
}