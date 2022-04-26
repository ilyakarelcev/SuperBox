using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDieHandler : MonoBehaviour, IPersonComponent
{
    [SerializeField] private CameraMove _cameraMove;

    public IPerson Person { get; private set; }

    public void Init(IPerson person)
    {
        Person = person;

        person.HealthManager.DieEvent += OnDie;
    }

    public void OnDie()
    {
        Person.Rigidbody.isKinematic = true;
        _cameraMove.SetEnabled(false);
    }
}
