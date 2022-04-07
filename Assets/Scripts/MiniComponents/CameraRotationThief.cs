using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotationThief : MonoBehaviour
{
    public Transform CameraTransform;
    public bool GetMain = true;

    public void Start()
    {
        if(GetMain)
            CameraTransform = Camera.main.transform;
    }

    public void LateUpdate()
    {
        transform.rotation = CameraTransform.rotation;
    }
}
