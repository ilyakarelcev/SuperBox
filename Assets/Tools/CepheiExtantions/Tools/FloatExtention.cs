using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cephei
{
    public static class FloatExtention
    {
        public static float Sign(this float f) => Mathf.Sign(f);

        public static float Abs(this float f) => Mathf.Abs(f);

        public static int ToInt(this float f) => Mathf.RoundToInt(f);

        public static float InDeltaTime(this float f) => f * Time.deltaTime;

        public static float Sqr(this float f) => f * f * f.Sign();

        public static float Clamp(this float f, float min, float max) => Mathf.Clamp(f, min, max);

        public static float Clamp01(this float f) => Mathf.Clamp01(f);

        public static float WithRandomOffset(this float f, float offset) => f + offset * Random.value * CustomRandom.Sign();
    }
}