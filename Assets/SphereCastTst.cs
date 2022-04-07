using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SphereCastTst : MonoBehaviour
{
    public float Radius;

    public ShortAttacker Attack;

    [ContextMenu("Test cast")]
    public void TestCast()
    {
        Collider[] colliders =
            Physics.SphereCastAll(transform.position, Radius, Vector3.up, 0.05f).Select(x => x.collider).ToArray();

        foreach (var collider in colliders)
        {
            Debug.Log("ColliderName: " + collider.name);
            if (collider.attachedRigidbody)
                Debug.Log("Rigidbody name: " + collider.attachedRigidbody.name);
        }
    }

    [ContextMenu("Test overlap")]
    public void TestOverlap()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, Radius);

        foreach (var collider in colliders)
        {
            Debug.Log("ColliderName: " + collider.name);
            if (collider.attachedRigidbody)
                Debug.Log("Rigidbody name: " + collider.attachedRigidbody.name);
        }
    }

    public static Collider[] OverlapSpher(Vector3 origin, float radius)
    {
        return Physics.OverlapSphere(origin, radius);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow - new Color(0, 0, 0, 0.7f);
        Gizmos.DrawSphere(transform.position, Radius);
    }
}
