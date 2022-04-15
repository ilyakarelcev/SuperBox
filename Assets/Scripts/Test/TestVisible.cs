using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVisible : MonoBehaviour
{
    public bool VisibleFlag;

    private void OnBecameInvisible()
    {
        VisibleFlag = false;
    }

    private void OnBecameVisible()
    {
        VisibleFlag = true;
    }
}
