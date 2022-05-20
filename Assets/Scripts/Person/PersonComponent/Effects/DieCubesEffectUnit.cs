using Cephei;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieCubesEffectUnit : MonoBehaviour
{
    public Collider Collider;
    public Rigidbody Rb;
    public Transform Transform;

    [HideInInspector] public float TimeToMove;
    [HideInInspector] public float Speed;
    [HideInInspector] public bool IsMove;

    public void Create(Vector3 velocity, Vector3 torque)
    {
        gameObject.SetActive(true);

        Rb.AddForce(velocity, ForceMode.VelocityChange);
        Rb.AddTorque(torque, ForceMode.VelocityChange);
    }

    public void InitToMove(float timeToMove, float speed)
    {
        TimeToMove = timeToMove;
        Speed = speed;
    }

    public bool MoveToTarger(Vector3 target)
    {
        if (IsMove == false)
        {
            Collider.enabled = false;
            Rb.isKinematic = true;
        }

        IsMove = true;

        Transform.MoveTowards(target, Speed * Time.deltaTime);

        if((Transform.position - target).sqrMagnitude < 0.1f * 0.1f)
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }
}
