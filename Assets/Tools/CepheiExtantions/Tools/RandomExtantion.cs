using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cephei
{
    public static class RandomExtantion
    {
        public static int Sign(this System.Random random)
        {
            if (UnityEngine.Random.value > 0.5f) return 1;

            return -1;
        }

        public static float Value(this System.Random random) => UnityEngine.Random.value;
    }

    public static class CustomRandom
    {
        public static float Sign()
        {
            if (UnityEngine.Random.value > 0.5f) return 1;

            return -1;
        }
    }

    public class SmartRandom
    {
        private Sector[] _sectors;
        private float _fine;

        /// <summary>
        /// Recomended accuracy: 2 - 10, fine: 1.5 - 7. The higher the accuracy, the closer the value will be to the input, the higher the penalty, the less likely it is that a similar value will fall out again
        /// </summary>
        /// <param name="accuracy"></param>
        /// <param name="fine"></param>
        public SmartRandom(int accuracy = 5, float fine = 2)
        {
            _fine = fine;
            _sectors = new Sector[accuracy];

            float sectorSize = (float)1 / accuracy;
            float sectorMin = 0;
            for (int i = 0; i < accuracy; i++)
            {
                _sectors[i] = new Sector(sectorMin, sectorMin += sectorSize);
            }
        }

        /// <summary>
        /// inputValue mast to be in range from 0 to 1
        /// </summary>
        /// <param name="value"></param>
        public float GetValueByInput(float inputValue)
        {
            if (inputValue < 0 || inputValue > 1)
            {
                Debug.LogError("Not Correctly Value");
                return inputValue;
            }

            return GetValue(inputValue);
        }

        public float GetValue()
        {
            return GetValue(UnityEngine.Random.value);
        }

        private float GetValue(float inputValue)
        {
            float lastMasses = 0;

            for (int i = 0; i < _sectors.Length; i++)
            {
                if (inputValue < lastMasses + _sectors[i].Mass)
                {
                    float takedMass = _sectors[i].Mass - (_sectors[i].Mass / _fine);
                    _sectors[i].Mass /= _fine;
                    RedistributeMasses(takedMass, i);
                    return _sectors[i].GetNumber;
                }

                lastMasses += _sectors[i].Mass;
            }

            return _sectors[_sectors.Length - 1].GetNumber;
        }

        public void RedistributeMasses(float addedMass, int excessSector)
        {
            float massForOneSector = addedMass / (_sectors.Length - 1);
            for (int i = 0; i < _sectors.Length; i++)
            {
                if (i == excessSector)
                    continue;

                _sectors[i].Mass += massForOneSector;
            }
        }

        private struct Sector
        {
            private Vector2 _range;
            public float GetNumber => _range.GetRandomValue();

            public float Mass;

            public Sector(float min, float max)
            {
                _range = new Vector2(min, max);
                Mass = max - min;
            }
        }
    }
}