using UnityEngine;
using Cephei.StateMachine;

[System.Serializable]
public class GoToTargetPattern : StateMachinePatternBase, IPersonComponent
{
    public Vector3 Target;
    [Space] 
    [SerializeField] private MoverBase _mover;

    public IPerson Person { get; private set; }

    public void Init(IPerson person)
    {
        Person = person;
        _mover = person.GetPersonComponentIs<MoverBase>();
    }

    public override void Activate()
    {
        base.Activate();
        _mover.StartMove();
        _mover.SetTarget(Target);
        _mover.ComeToTarget += InvokeEndWorkEvent;
    }

    public override void DeActivate()
    {
        base.DeActivate();
        _mover.ComeToTarget -= InvokeEndWorkEvent;
    }
}
