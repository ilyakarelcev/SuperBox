using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateVisibleObjectTest : MonoBehaviour
{
    public bool TestInstantiate;
    public TestVisible Prefab;

    private void Update()
    {
        if (TestInstantiate)
        {
            TestInstantiate = false;

            Instantiate(Prefab, transform.position, Quaternion.identity);
        }
    }
}
