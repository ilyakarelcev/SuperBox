using Cephei.StateMachine;
using UnityEngine;

[System.Serializable]
public class ArcherAttackPattern : StateMachinePatternBase, IPersonComponent
{
    public IMover Mover;

    public IAttackView _attackView;
    private Shooter _attaker;

    private IAttackHandler[] _handlers;

    public IPerson Person { get; private set; }

    public void Init(IPerson person)
    {
        Person = person;

        _attackView = person.GetPersonComponentIs<IAttackView>();
        _attaker = person.GetPersonComponent<Shooter>();

        if (person.GetAllPersonComponentIs(out _handlers) == false)
            Debug.LogError($"Not attack handlers on {Person.Transform.name} person");


        _attaker.AttackEvent += HandleAttack;
    }

    public override void Activate()
    {
        base.Activate();

        _attackView.StartAttack();
        Mover.StartMove();

        _attackView.BeginingOfDamageEvent += OnAttack;
        _attackView.EndAttackEvent += EndAttack;
    }

    public override void DeActivate()
    {
        base.DeActivate();

        _attackView.BeginingOfDamageEvent -= OnAttack;
        _attackView.EndAttackEvent -= EndAttack;

        _attackView.BreakAttack();

        # region Not unsubscribe at Shooter
        // Не отписываюсь при от попадания пуль при деактивации. Если этот паттерн один пользуется Shooter то все норм, но если не один, тогда это создаст проблеммы, так как подписки будут копиться
        // В принципе можно просто через сам шутер это делать. Мы например когда ему говорим стрельнуть, еще и говорим а подпиши ка наши хендлеры на свою пулю. Но это нарушает принцип подстановки Барабары Лисков
        #endregion
    }

    private void OnAttack()
    {
        Mover.StopMove();
        _attaker.Attack();
    }

    private void EndAttack()
    {
        InvokeEndWorkEvent();
    }

    private void HandleAttack(Attack attack)
    {
        if (attack.AttackedPerson.GetType() != typeof(Player) && BalanceSetup.EnemyHitToEnemy== false) 
            return;

        attack.AttackingPerson = Person;
        foreach (var handler in _handlers)
        {
            handler.Handle(attack);
        }

        attack.AttackedPerson.AttackTakerManager.TakeAttack(attack);
    }
}