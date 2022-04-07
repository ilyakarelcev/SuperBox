using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CircleEffect : IATtackViewSuport, IPersonComponent
{
    [SerializeField] private ParticleSystem _startAttackEffect;

    public IPerson Person { get; private set; }

    private IAttackView _attackView;

    [Space]
    public bool Log;

    public void Init(IPerson person)
    {
        Person = person;

        Init(person.GetPersonComponentIs<IAttackView>());
    }

    public void Init(IAttackView attackView)
    {
        _attackView = attackView;

        _attackView.StartAttackEvent += OnStartAttack;
        _attackView.EndAttackEvent += OnEndAttack;
    }

    public void OnStartAttack()
    {
        _startAttackEffect.Play();

        if (Log) Debug.Log("StartAttack");
    }

    public void OnEndAttack()
    {
        _startAttackEffect.Stop();

        if (Log) Debug.Log("EndAttack");
    }
}