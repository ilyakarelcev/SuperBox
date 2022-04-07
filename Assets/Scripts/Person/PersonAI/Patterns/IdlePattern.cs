using Cephei.StateMachine;
using UnityEngine;

[System.Serializable]
public class IdlePattern : StateMachinePatternBase, IPersonComponent
{
    public MoverBase _mover;
    public MoverBase _rotator;

    public IPerson Person { get; private set; }

    private Vector3 _startRotation;

    public void Init(IPerson person)
    {
        Person = person;

        _startRotation = Person.Forward;
    }

    public override void Activate()
    {
        base.Activate();
        _mover.StopMove();
        _rotator.StartMove();
        _rotator.SetTarget(Person.Position + _startRotation);
    }

    public override void DeActivate()
    {
        base.DeActivate();
    }
}