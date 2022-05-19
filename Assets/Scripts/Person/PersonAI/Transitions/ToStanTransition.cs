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
        _attackTakerManager.OnTakeAttack += OnAttackTake;
    }

    public override void DeActivate()
    {
        _healthManager.ApplyDamageEvent -= OnApplyDamage;
        _attackTakerManager.OnTakeAttack -= OnAttackTake;
    }

    private void OnApplyDamage(float damage)
    {
        if (damage > DamageToStan)
            ActivatePattern();
    }

    private void OnAttackTake(Attack attack)
    {
        if (attack is CircleImpulsAttack)
            ActivatePattern();

        return;

        if (attack.Impuls.sqrMagnitude > ImpulsToStan.Sqr())
            ActivatePattern();
    }
}
