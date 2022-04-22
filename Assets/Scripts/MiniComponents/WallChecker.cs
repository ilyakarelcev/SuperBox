using Cephei;
using System;
using UnityEngine;

public class WallChecker : MonoBehaviour
{
    public event Action OnDetectWall;

    public void Active()
    {
        enabled = true;
    }

    public void Deactive()
    {
        enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Walls"))
            OnDetectWall?.Invoke();            
    }
}