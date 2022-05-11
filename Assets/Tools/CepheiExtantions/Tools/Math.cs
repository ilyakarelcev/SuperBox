using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cephei
{
    [Serializable]
    public static class Math
    {
        public static float LerpByCurve(float from, float to, AnimationCurve curve, float t)
        {
            float lengh = to - from;
            return from + lengh * curve.Evaluate(t);
        }
    }
}