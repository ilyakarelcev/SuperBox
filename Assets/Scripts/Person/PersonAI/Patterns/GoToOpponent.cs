using Cephei;
using Cephei.StateMachine;
using System;
using UnityEngine;

[System.Serializable]
public class GoToOpponentPattern : StateMachinePatternBase, IPersonComponent
{
    public MoverBase Mover;

    [SerializeField] private float _targetDistanceToPlayer = 0.5f;
    [Space]
    [SerializeField] private Transform _opponent;

    public Action ComeToOpponent;

    public IPerson Person { get; private set; }

    private CastomCoroutine _workCoroutine;

    public void Init(IPerson person)
    {
        Person = person;
    }

    public override void Activate()
    {
        base.Activate();
        Mover.StartMove();
        _workCoroutine = Person.Operator.OpenUpdateCoroutine(Work, LifeType.Cycle);
    }

    public override void DeActivate()
    {
        base.DeActivate();
        _workCoroutine.Destroy();
        _workCoroutine = null;
    }

    void Work()
    {
        Vector3 fromPlayer = Person.Position - _opponent.position;
        Mover.SetTarget(_opponent.position);

        if (fromPlayer.sqrMagnitude < _targetDistanceToPlayer.Sqr())
            InvokeEndWorkEvent();
    }
}
