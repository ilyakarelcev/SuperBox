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

        public static float Sqr(this float f, int deg)
        {
            float sign = f.Sign();
            float result = f;
            for (int i = 1; i < deg; i++)
            {
                result *= f;
            }
            return result * sign;
        }


        public static float Clamp(this float f, float min, float max) => Mathf.Clamp(f, min, max);

        public static float Clamp01(this float f) => Mathf.Clamp01(f);


        public static float WithRandomOffset(this float f, float offset) => f + offset * Random.value * CustomRandom.Sign();

        public static float WithRandomOffset(this float f, float offset, bool isPositiveOffset) => f + offset * Random.value * (isPositiveOffset ? 1 : -1);


        public static float WithRandomPercent(this float f, float percent) 
            => f + (f * (percent * Random.value / 100) * CustomRandom.Sign());

        public static float WithRandomPercent(this float f, float percent, bool isPositiveOffset) 
            => f.WithRandomPercent(percent).Abs() * (isPositiveOffset ? 1 : -1);
    }
}