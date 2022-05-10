using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieHandler : MonoBehaviour, IPersonComponent
{
    [SerializeField] private GameObject _mainObject;
    [Space]
    [SerializeField] private Collider[] _colliders;
    [SerializeField] private GameObject[] _objects;
    [Space]
    [SerializeField] private DieCubesEffect _cubesEffect;
    [SerializeField] private ParticleSystem _effect;

    public IPerson Person { get; private set; }

    public void Init(IPerson person)
    {
        Person = person;

        person.HealthManager.DieEvent += OnDie;
        _cubesEffect.Init(Person);
    }

    public void OnDie()
    {
        foreach (var collider in _colliders)
        {
            collider.enabled = false;
        }
        foreach (var @object in _objects)
        {
            @object.SetActive(false);
        }

        _cubesEffect.EndAnimationEvent += DestroyPerson;
        _cubesEffect.OnDie();

        _effect.Play();
    }

    private void DestroyPerson()
    {
        Destroy(_mainObject);
    }
}
