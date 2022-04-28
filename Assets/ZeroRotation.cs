using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroRotation : MonoBehaviour
{

    void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }
}
