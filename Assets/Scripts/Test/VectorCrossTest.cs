using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class VectorCrossTest : MonoBehaviour
{
    public Vector3 vector1;
    public Vector3 vector2;
    
    void Update()
    {
        Debug.DrawRay(transform.position, vector1, Color.blue);
        Debug.DrawRay(transform.position, vector2, Color.green);

        Vector3 cross = Vector3.Cross(vector1, vector2);

        Debug.DrawRay(transform.position, cross, Color.red);
    }
}
