using Cephei.StateMachine;
using UnityEngine;

[System.Serializable]
public class ShortAttackPattern : StateMachinePatternBase, IPersonComponent
{
    public IMover Mover;

    private IAttackView _attackView;
    private IAttacker _attacker;

    private IAttackHandler[] _handlers;

    public IPerson Person { get; private set; }

    public void Init(IPerson person)
    {
        Person = person;

        _attackView = person.GetPersonComponentIs<IAttackView>();
        _attacker = person.GetPersonComponentIs<IAttacker>();

        if (person.GetAllPersonComponentIs(out _handlers) == false)
            Debug.LogError($"Not attack handlers on {Person.Transform.name} person");
    }

    public override void Activate()
    {
        base.Activate();

        Mover.StopMove();

        _attackView.BeginingOfDamageEvent += OnBeginingOfDamage;
        _attackView.EndingOfDamageEvent += OnEndingOfDamage;
        _attackView.EndAttackEvent += EndAttack;

        _attacker.FindPersonEvent += HandleAttack;

        _attackView.StartAttack();
    }

    public override void DeActivate()
    {
        base.DeActivate();

        _attackView.BreakAttack();

        _attackView.BeginingOfDamageEvent -= OnBeginingOfDamage;
        _attackView.EndingOfDamageEvent -= OnEndingOfDamage;
        _attackView.EndAttackEvent -= EndAttack;

        _attacker.FindPersonEvent -= HandleAttack;
    }

    private void EndAttack()
    {
        InvokeEndWorkEvent();
    }

    private void OnBeginingOfDamage()
    {
        _attacker.Direction = Person.Forward;
        _attacker.StartAttack();
    }

    private void OnEndingOfDamage()
    {
        _attacker.EndAttack();
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