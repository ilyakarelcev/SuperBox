using Cephei.StateMachine;
using UnityEngine;

[System.Serializable]
public class ShortAttackPattern : StateMachinePatternBase, IPersonComponent
{
    public IMover Mover;

    private IDontBreakerAttackView _attackView;
    private ShortAttacker _attacker;
    private IDontBreakerAttackHandler _DontBreakerAttackHandler;

    private IAttackHandler[] _handlers;

    public IPerson Person { get; private set; }

    public void Init(IPerson person)
    {
        Person = person;

        _attackView = person.GetPersonComponentIs<IDontBreakerAttackView>();
        _attacker = person.GetPersonComponent<ShortAttacker>();

        _DontBreakerAttackHandler = person.GetPersonComponentIs<IDontBreakerAttackHandler>();

        if (person.GetAllPersonComponentIs(out _handlers) == false)
            Debug.LogError($"Not attack handlers on {Person.Transform.name} person");
    }

    public override void Activate()
    {
        base.Activate();

        Mover.StopMove();
        _attackView.StartAttack();

        _attackView.BeginingOfDamageEvent += HandleAttacker;
        _attackView.EndAttackEvent += EndAttack;

        _attackView.BeginingDontBreakStateEvent += _DontBreakerAttackHandler.StartBreaker;
        _attackView.EndingDontBreakStateEvent += _DontBreakerAttackHandler.EndBreaker;

        _attacker.AttackEvent += HandleAttack;
    }

    public override void DeActivate()
    {
        base.DeActivate();

        _attackView.BeginingOfDamageEvent -= HandleAttacker;
        _attackView.EndAttackEvent -= EndAttack;

        _attackView.BeginingDontBreakStateEvent -= _DontBreakerAttackHandler.StartBreaker;
        _attackView.EndingDontBreakStateEvent -= _DontBreakerAttackHandler.EndBreaker;

        _attackView.BreakAttack();

        _attacker.AttackEvent -= HandleAttack;
    }

    private void EndAttack()
    {
        InvokeEndWorkEvent();
    }

    private void HandleAttacker()
    {
        _attacker.Attack(Person.Forward);
    }

    private void HandleAttack(Attack attack)
    {
        if (attack.AttackedPerson.GetType() != typeof(Player) && BalanceSetup.EnemyHitToEnemy == false) return;

        attack.AttackingPerson = Person;
        foreach (var handler in _handlers)
        {
            handler.Handle(attack);
        }

        attack.AttackedPerson.AttackTakerManager.TakeAttack(attack);
    }
}