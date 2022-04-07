using System;
using System.Collections.Generic;

namespace Cephei
{
    public static class RandomExtantion
    {
        public static int Sign(this Random random)
        {
            if (UnityEngine.Random.value > 0.5f) return 1;

            return -1;
        }

        public static float Value(this Random random) => UnityEngine.Random.value;
    }

    public static class CustomRandom
    {
        public static float Sign()
        {
            if (UnityEngine.Random.value > 0.5f) return 1;

            return -1;
        }
    }
}