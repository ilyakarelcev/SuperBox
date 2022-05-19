using Cephei;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartRandomTest : MonoBehaviour
{
    public float[] SimpleRandom = new float[15];
    public float[] SmartRandom = new float[15];


    void Start()
    {
        SmartRandom random = new SmartRandom(5, 4);

        SimpleRandom = new float[15];
        SmartRandom = new float[15];

        for (int i = 0; i < SimpleRandom.Length; i++)
        {
            SimpleRandom[i] = Random.value * 100;
        }

        for (int i = 0; i < SmartRandom.Length; i++)
        {
            SmartRandom[i] = random.GetValueByInput(SimpleRandom[i] / 100) * 100;
        }

        CustomDebug.Break();
    }
}

