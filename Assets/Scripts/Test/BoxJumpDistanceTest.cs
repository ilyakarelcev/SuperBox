using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxJumpDistanceTest : MonoBehaviour
{
    public bool Teleport;
    public bool UseKeyDown = true;
    [Space]
    public Transform StartPoint;
    public Transform Box;

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.J) && UseKeyDown) || Teleport)
            Box.position = StartPoint.position + Vector3.up * 0.5f;

        Teleport = false;
    }
}
