using Cephei;
using Cephei.StateMachine;
using UnityEngine;

[System.Serializable]
public class TransitionOnCircleAbility : TransitionBase
{
    private IAttackTakerManager _attackTakerManager;

    public void Init(IStateMachinePattern nextPattern, IStateMachine stateMachine, IAttackTakerManager attackTakerManager)
    {
        _nextPattern = nextPattern;
        _stateMachine = stateMachine;

        _attackTakerManager = attackTakerManager;
    }

    public override void Activate()
    {
        _attackTakerManager.AttackTakersForEachEndEvent += OnEndAttackTake;
    }

    public override void DeActivate()
    {
        _attackTakerManager.AttackTakersForEachEndEvent -= OnEndAttackTake;
    }

    private void OnEndAttackTake(Attack attack)
    {
        if (attack is CircleImpulsAttack)
            ActivatePattern();
    }
}
