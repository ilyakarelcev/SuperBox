using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CircleEffectForDontBreakAttack : IDontBreakerAttackViewSuport, IPersonComponent
{
    [SerializeField] private ParticleSystem _startAttackEffect;
    [SerializeField] private ParticleSystem _dontBreakerAttackEffect;

    public IPerson Person { get; private set; }

    private IDontBreakerAttackView _attackView;

    [Space]
    public bool Log;

    public void Init(IPerson person)
    {
        Person = person;

        Init(person.GetPersonComponentIs<IDontBreakerAttackView>());
    }

    public void Init(IDontBreakerAttackView attackView)
    {
        _attackView = attackView;

        _attackView.StartAttackEvent += OnStartAttack;
        _attackView.BeginingDontBreakStateEvent += OnBeginingDontBreakState;
        _attackView.EndAttackEvent += OnEndAttack;
    }

    public void OnStartAttack()
    {
        _startAttackEffect.Play();

        if (Log) Debug.Log("StartAttack");
    }

    public void OnBeginingDontBreakState()
    {
        _startAttackEffect.Stop();
        _dontBreakerAttackEffect.Play();

        if (Log) Debug.Log("BeginingDontBreakState");
    }

    public void OnEndAttack()
    {
        _startAttackEffect.Stop();
        _dontBreakerAttackEffect.Stop();

        if (Log) Debug.Log("EndAttack");
    }
}