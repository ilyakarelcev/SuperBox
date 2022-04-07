using Cephei;
using Cephei.StateMachine;
using UnityEngine;

[System.Serializable]
public class ToStanTransition : TransitionBase
{
    public float DamageToStan = 5;
    public float ImpulsToStan = 12;

    private HealthManager _healthManager;
    private IAttackTakerManager _attackTakerManager;

    public void Init(IStateMachinePattern nextPattern, IStateMachine stateMachine, HealthManager healthManager, IAttackTakerManager attackTakerManager)
    {
        _nextPattern = nextPattern;
        _stateMachine = stateMachine;

        _healthManager = healthManager;
        _attackTakerManager = attackTakerManager;
    }

    public override void Activate()
    {
        _healthManager.ApplyDamageEvent += OnApplyDamage;
        _attackTakerManager.AttackTakersForEachEndEvent += OnEndAttackTake;
    }

    public override void DeActivate()
    {
        _healthManager.ApplyDamageEvent -= OnApplyDamage;
        _attackTakerManager.AttackTakersForEachEndEvent -= OnEndAttackTake;
    }

    private void OnApplyDamage(float damage)
    {
        if (damage > DamageToStan)
            ActivatePattern();
    }

    private void OnEndAttackTake(Attack attack)
    {
        if (attack.Impuls.sqrMagnitude > ImpulsToStan.Sqr())
            ActivatePattern();
    }
}
