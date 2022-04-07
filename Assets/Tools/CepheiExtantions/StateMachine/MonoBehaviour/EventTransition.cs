using Cephei.StateMachine;
using Cephei.StateMachine.MonoBehaviour;
using UnityEngine;

public class EventTransition : TransitionBaseMono
{
    [SerializeField] private StateMachineMono _stateMachine;

    public bool IsActive { get; private set; }

    public override void Activate()
    {
        IsActive = true;
    }

    public override void DeActivate()
    {
        IsActive = false;
    }

    public void OnEvent()
    {
        if (IsActive)
            _stateMachine.ActivatePattern(_nextPattern);
    }
}