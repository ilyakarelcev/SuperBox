using Cephei;
using System;
using UnityEngine;

public class EnemyVision : MonoBehaviour, IPersonComponent
{
    [SerializeField] private MeshRenderer _circle;

    public bool IsPlayerInside { get; private set; }

    public IPerson Person { get; private set; }

    public event Action<bool> ChangeIsPlayerVisionEvent;

    public void Init(IPerson person)
    {
        Person = person;
    }

    public void SetRadius(float radius)
    {
        transform.localScale = Vector3.one * radius;
    }

    private void LateUpdate()
    {
        transform.position = Person.Position.ZeroY() + Vector3.up * 0.01f;
        transform.rotation = Person.Forward.ZeroY().GetRotation();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayerInside) return;

        CheckPlayerVision(true, other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsPlayerInside == false) return;

        CheckPlayerVision(false, other);
    }

    private void CheckPlayerVision(bool visionStaus, Collider other)
    {
        if (IPerson.GetPersonFromRigidbody(other.attachedRigidbody) is Player)
        {
            IsPlayerInside = visionStaus;
            ChangeIsPlayerVisionEvent?.Invoke(visionStaus);

            if (visionStaus)
                _circle.enabled = false;
        }
    }
}
