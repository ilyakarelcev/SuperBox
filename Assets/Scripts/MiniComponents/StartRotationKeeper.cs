using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRotationKeeper : MonoBehaviour
{
    private Quaternion _startRotation;

    void Start()
    {
        _startRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        transform.rotation = _startRotation;
    }
}
