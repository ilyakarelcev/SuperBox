using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLocalPositionKeeper : MonoBehaviour
{
    public Transform Origin;

    private Vector3 _startLocalPosition;

    void Start()
    {
        _startLocalPosition = transform.position - Origin.position;
    }

    private void LateUpdate()
    {
        transform.position = Origin.position + _startLocalPosition;
    }
}