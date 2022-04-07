using Cephei.StateMachine;
using UnityEngine;

[System.Serializable]
public class StopAndWaitPattern : StateMachinePatternBase, IPersonComponent
{
    public IMover Mover;

    [SerializeField] private float _waitTime = 1;

    public IPerson Person { get; private set; }    

    private CastomCoroutine _waitOperation;

    public void Init(IPerson person)
    {
        Person = person;
    }

    public override void Activate()
    {
        base.Activate();
        Mover.StopMove();

        _waitOperation = Person.Operator.OpenCoroutineWithTimeStep(InvokeEndWorkEvent, _waitTime, LifeType.OneShot);
    }

    public override void DeActivate()
    {
        base.DeActivate();

        _waitOperation?.Destroy();
        _waitOperation = null;
    }
}