using Cephei.StateMachine;
using UnityEngine;

[System.Serializable]
public class WaitAfterAttackPattern : StateMachinePatternBase, IPersonComponent
{
    public IMover Mover;
    public IMover Rotator;

    [SerializeField] private float _waitTime = 1;
    [Space]
    [SerializeField] private Transform _opponent;

    public IPerson Person { get; private set; }    

    private CastomCoroutine _waitOperation;
    private CastomCoroutine _updateTarget;

    public void Init(IPerson person)
    {
        Person = person;
    }

    public override void Activate()
    {
        base.Activate();
        Mover.StopMove();
        Rotator.StartMove();

        _waitOperation = Person.Operator.OpenCoroutineWithTimeStep(InvokeEndWorkEvent, _waitTime, LifeType.OneShot);
        _updateTarget = Person.Operator.OpenUpdateCoroutine(Work, LifeType.Cycle);
    }

    public override void DeActivate()
    {
        base.DeActivate();

        _waitOperation?.Destroy();
        _waitOperation = null;

        _updateTarget.Destroy();
        _updateTarget = null;
    }

    void Work()
    {
        Rotator.SetTarget(_opponent.position);
    }
}