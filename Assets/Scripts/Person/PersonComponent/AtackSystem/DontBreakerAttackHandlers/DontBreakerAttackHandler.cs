using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DontBreakerAttackHandler : IDontBreakerAttackHandler, IPersonComponent
{
    public float DamageToBreak = 9;

    public IPerson Person { get; private set; }

    public event Action BreakEvent;

    private IAttackTakerManager _attackTaker;

    private IAttackTaker[] _defaultEffect;
    private IAttackTaker[] _dontBreakStateEfect;

    [Space]
    public bool Log;

    public void Init(IPerson person)
    {
        Person = person;

        _attackTaker = person.AttackTakerManager;
    }

    public void Init(IAttackTaker[] defaultEffect, IAttackTaker[] dontBreakStateEfect)
    {
        _defaultEffect = defaultEffect;
        _dontBreakStateEfect = dontBreakStateEfect;
    }

    public void StartBreaker()
    {
        Person.Rigidbody.isKinematic = true;

        SwapEffect(_dontBreakStateEfect, _defaultEffect);

        _attackTaker.Conditions.Add(AttackCondition);
    }

    public void EndBreaker()
    {
        Person.Rigidbody.isKinematic = false;

        SwapEffect(_defaultEffect, _dontBreakStateEfect);

        _attackTaker.Conditions.Remove(AttackCondition);
    }

    private bool AttackCondition(Attack attack)
    {
        if (Log) Debug.Log("Damage befor condition: " + attack.Damage);

        if (attack.Damage < DamageToBreak)
        {
            attack.Damage = 0;
            attack.Impuls = default;
            return true;
        }

        _attackTaker.ConditionsForEachEndEvent += DelayEnd;
        BreakEvent?.Invoke();
        return true;
    }

    private void DelayEnd(Attack attack)
    {
        EndBreaker();
        _attackTaker.ConditionsForEachEndEvent -= DelayEnd;
    }

    private void SwapEffect(IAttackTaker[] toAdd, IAttackTaker[] toRemove)
    {
        foreach (var taker in toRemove)
        {
            _attackTaker.AttackTakers.Remove(taker);
        }
        _attackTaker.AttackTakers.AddRange(toAdd);
    }
}