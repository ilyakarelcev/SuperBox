using System;
using UnityEngine;

public class CollisionsEvents : MonoBehaviour
{
    public event Action<Collision> EnterCollisionEvent;
    public event Action<Collision> StayCollisionEvent;
    public event Action<Collision> ExitCollisionEvent;

    private void OnCollisionEnter(Collision collision)
    {
        EnterCollisionEvent.Invoke(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        StayCollisionEvent.Invoke(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        ExitCollisionEvent.Invoke(collision);
    }
}
