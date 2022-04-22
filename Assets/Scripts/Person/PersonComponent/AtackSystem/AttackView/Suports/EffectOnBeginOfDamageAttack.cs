using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectOnBeginOfDamageAttack : IATtackViewSuport, IPersonComponent
{
    [SerializeField] private ParticleSystem _startAttackEffect;

    public IPerson Person { get; private set; }

    private IAttackView _attackView;

    private bool _isEndOfDamage;

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

        _attackView.BeginingOfDamageEvent += OnBeginOfDamage;
        _attackView.EndingOfDamageEvent += () => _isEndOfDamage = true;
        _attackView.EndAttackEvent += OnEndAttack;
    }

    public void OnBeginOfDamage()
    {
        _startAttackEffect.Play();

        if (Log) Debug.Log("StartAttack");
    }

    private void OnEndAttack()
    {
        if (_isEndOfDamage == false)
            _startAttackEffect.Stop();
    }
}