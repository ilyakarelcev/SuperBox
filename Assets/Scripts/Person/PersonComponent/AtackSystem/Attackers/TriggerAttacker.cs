using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAttacker : MonoBehaviour, IAttacker
{
    [SerializeField] private Collider[] _colliders;

    public event Action<IPerson, float> AttackEvent;

    public bool IsActive { get; private set; }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        SetEnabledOfColliders(false);
    }

    public void Active()
    {
        SetEnabledOfColliders(true);
    }

    public void Deactive()
    {
        SetEnabledOfColliders(false);
    }

    public void Attack() { }

    private void OnTriggerEnter(Collider other)
    {
        IPerson person = IPerson.GetPersonFromRigidbody(other.attachedRigidbody);

        if (person != null)
            AttackEvent.Invoke(person, 1);
    }

    private void SetEnabledOfColliders(bool enabled)
    {
        if (IsActive == enabled) return;
        IsActive = enabled;

        foreach (var collider in _colliders)
        {
            collider.enabled = enabled;
        }
    }
}
