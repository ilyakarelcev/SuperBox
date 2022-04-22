using Cephei;
using Cephei.StateMachine;
using System;
using UnityEngine;

[System.Serializable]
public class TriceratopsGoToOpponentPattern : StateMachinePatternBase, IPersonComponent
{
    public IMover PersonMover;
    public IMover Rotator;

    [SerializeField] private float _targetDistanceToPlayer = 8;
    [SerializeField] private float _angleToTargetForAttack = 25;
    [Space]
    [SerializeField] private Transform _opponent;

    public Action ComeToOpponent;

    public IPerson Person { get; private set; }

    private CastomCoroutine _workCoroutine;


    [Space]
    public float DistanceView;
    public float AngleView;

    public bool IsFinaly;

    public void Init(IPerson person)
    {
        Person = person;
    }

    public override void Activate()
    {
        IsFinaly = false; ///



        base.Activate();

        PersonMover.SetTarget(_opponent);
        PersonMover.StartMove();


        _workCoroutine = Person.Operator.OpenUpdateCoroutine(Work, LifeType.Cycle);
    }

    public override void DeActivate()
    {
        IsFinaly = true; ////


        base.DeActivate();
        _workCoroutine.Destroy();
        _workCoroutine = null;
    }

    void Work()
    {
        Vector3 toPlayer = _opponent.position - Person.Position;


        DistanceView = toPlayer.magnitude;
        AngleView = Vector3.Angle(toPlayer.ZeroY(), Person.Forward.ZeroY());


        if (toPlayer.sqrMagnitude < _targetDistanceToPlayer.Sqr())
        {
            PersonMover.StopMove();
            Rotator.StartMove();

            if (Vector3.Angle(toPlayer.ZeroY(), Person.Forward.ZeroY()) < _angleToTargetForAttack)
                InvokeEndWorkEvent();

            return;
        }        
        PersonMover.StartMove();
    }
}
