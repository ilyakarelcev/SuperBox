using Cephei.StateMachine;
using UnityEngine;

[System.Serializable]
public class TriceratopsAttackPattern : StateMachinePatternBase, IPersonComponent
{
    [Range(0, 1)] public float MultiplyOnStay = 0.6f;
    [Range(0, 1)] public float MultiplyOnJerk = 1;

    public SimpleMover MoverForAttack;
    public IMover Rotator;
    public IMover PersonMover;

    public Transform Opponent;

    public TriceratopsAttackView _attackView;
    public WallChecker WallChecker;
    private IAttacker _attacker;

    private IAttackHandler[] _handlers;

    public IPerson Person { get; private set; }

    private float _currentMultiply;

    public void Init(IPerson person)
    {
        Person = person;

        _attackView = person.GetPersonComponentIs<TriceratopsAttackView>();
        _attacker = person.GetPersonComponentIs<IAttacker>();

        if (person.GetAllPersonComponentIs(out _handlers) == false)
            Debug.LogError($"Not attack handlers on {Person.Transform.name} person");

        MoverForAttack.StopMove();
    }

    public override void Activate()
    {
        base.Activate();

        _attackView.BeginingOfDamageEvent += _attacker.StartAttack;
        _attackView.EndingOfDamageEvent += _attacker.EndAttack;

        _attackView.EndAttackEvent += EndAttack;
        _attackView.BeginJerkEvent += OnBeginingOfJerk;

        _attacker.FindPersonEvent += HandleAttack;

        PersonMover.StopMove();
        Rotator.SetTarget(Opponent);
        Rotator.StartMove();

        WallChecker.Active();
        WallChecker.OnDetectWall += EndAttack;

        Person.HealthManager.ApplyDamageEvent += OnApplyDamage;

        _attackView.StartAttack();

        _currentMultiply = MultiplyOnStay;
    }

    public override void DeActivate()
    {
        base.DeActivate();

        _attackView.BreakAttack();

        WallChecker.Deactive();
        WallChecker.OnDetectWall -= EndAttack;

        MoverForAttack.StopMove();


        _attackView.BeginingOfDamageEvent -= _attacker.StartAttack;
        _attackView.EndingOfDamageEvent -= _attacker.EndAttack;

        _attackView.EndAttackEvent -= EndAttack;
        _attackView.BeginJerkEvent -= OnBeginingOfJerk;

        Person.HealthManager.ApplyDamageEvent -= OnApplyDamage;
        Person.Rigidbody.isKinematic = false;

        _attacker.FindPersonEvent -= HandleAttack;        
    }

    private void EndAttack()
    {
        InvokeEndWorkEvent();
    }

    private void OnApplyDamage(float damage)
    {
        EndAttack();
    }

    private void OnBeginingOfJerk()
    {
        Rotator.StopMove();
        MoverForAttack.SetTarget(Person.Position + Person.Forward * 100);
        MoverForAttack.StartMove();

        Person.HealthManager.ApplyDamageEvent -= OnApplyDamage;
        Person.Rigidbody.isKinematic = true;
        Transition.Find(x => x is TransitionOnCircleAbility).DeActivate();

        _currentMultiply = MultiplyOnJerk;
    }

    private void HandleAttack(Attack attack)
    {
        if (attack.AttackedPerson.GetType() != typeof(Player) && BalanceSetup.EnemyHitToEnemy == false) return;

        attack.AttackingPerson = Person;
        attack.AttackMultiply *= _currentMultiply;
        foreach (var handler in _handlers)
        {
            handler.Handle(attack);
        }

        attack.AttackedPerson.AttackTakerManager.TakeAttack(attack);
    }
}
