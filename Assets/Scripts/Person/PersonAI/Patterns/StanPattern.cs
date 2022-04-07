using Cephei;
using Cephei.StateMachine;
using System;
using UnityEngine;

[System.Serializable]
public class StanPattern : StateMachinePatternBase, IPersonComponent
{
    [SerializeField] private float _timeInStan = 1;
    [Space]
    private AttackAnimationView _animationControler;
    public MoverBase Mover;

    public IPerson Person { get; private set; }

    public event Action OnEndStan;

    private CastomCoroutine _waitOperation;

    public void Init(IPerson person)
    {
        Person = person;

        _animationControler = person.GetPersonComponentIs<AttackAnimationView>();
    }

    public override void Activate()
    {
        base.Activate();

        //_animationControler.BreakAttack(); ////////
        Mover.StopMove();
        _waitOperation = Person.Operator.OpenCoroutineWithTimeStep(InvokeEndWorkEvent, _timeInStan, LifeType.OneShot);
    }

    public override void DeActivate()
    {
        base.DeActivate();

        _waitOperation?.Destroy();
        _waitOperation = null;
    }
}
