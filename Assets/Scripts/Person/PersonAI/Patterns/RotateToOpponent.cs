using Cephei.StateMachine;
using UnityEngine;

[System.Serializable]
public class RotateToOpponent : StateMachinePatternBase, IPersonComponent
{
    public IMover Mover;

    public IPerson Person { get; private set; }

    public void Init(IPerson person) { }

    public Transform Opponent;

    public override void Activate()
    {
        base.Activate();

        Mover.StartMove();
        Mover.SetTarget(Opponent);
        Mover.ComeToTarget += InvokeEndWorkEvent;
    }

    public override void DeActivate()
    {
        base.DeActivate();

        Mover.ComeToTarget -= InvokeEndWorkEvent;
    }
}
