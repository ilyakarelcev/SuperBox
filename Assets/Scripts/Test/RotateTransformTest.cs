using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTransformTest : MonoBehaviour
{
    public Transform Target;

    public MoverBase MoverBase;

    public void Update()
    {
        MoverBase.StartMove();
        MoverBase.SetTarget(Target.position);
    }
}
