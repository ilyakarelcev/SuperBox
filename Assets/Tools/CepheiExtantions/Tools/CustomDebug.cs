using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cephei
{
    public static class CustomDebug
    {
        public static void Break()
        {
            Debug.Break();
        }

        public static void DrawCross(Vector3 origin, float size, Color color)
        {
            Debug.DrawRay(origin - Vector3.up * size / 2, Vector3.up, color);
            Debug.DrawRay(origin - Vector3.right * size / 2, Vector3.right, color);
            Debug.DrawRay(origin - Vector3.forward * size / 2, Vector3.forward, color);
        }
    }
}
