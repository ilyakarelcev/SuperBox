using Cephei.StateMachine;
using UnityEngine;

[System.Serializable]
public class TriceratopsAI : IPersonComponent
{
    [SerializeField] private float _visionRadiusInIdle = 20;
    [SerializeField] private float _visionRadiusInAttack = 30;
    [Space]
    [SerializeField] private BehaviourSelector _behaviourSelector;

    [Header("AttackPattern")]
    [SerializeField] private TriceratopsAttackPattern _attackPattern;
    [SerializeField] private TriceratopsGoToOpponentPattern _goToOpponent; //// Change
    [SerializeField] private StopAndWaitPattern _waitPattern;
    [SerializeField] private PlayEffectPattern _startAttackPattern;

    public IPerson Person { get; private set; }

    public StateMachineLevle AttackBehaviour { get; private set; }
    public StateMachineLevle Behaviour { get; private set; }

    private EnemyVision _enemyVision;
    private LineToPlayerOnInvisible _lineToPlayer;
    private EnemyPointer _pointer;

    public void Init(IPerson person)
    {
        Person = person;

        _enemyVision = Person.GetPersonComponent<EnemyVision>();
        _enemyVision.ChangeIsPlayerVisionEvent += HandleVision;

        _lineToPlayer = person.GetPersonComponent<LineToPlayerOnInvisible>();
        _pointer = person.GetPersonComponent<EnemyPointer>();

        CreateAttackPattern();
        Behaviour = _behaviourSelector.GetBehaviour();
        (Behaviour as IPersonComponent).Init(person);

        Person.InitializeThisComponents(
            _attackPattern, _goToOpponent, _waitPattern, Behaviour as IPersonComponent);

        Behaviour.Activate();

        Person.Operator.OpenUpdateCoroutine(TestUpdate, LifeType.Cycle);

        person.HealthManager.DieEvent += OnDie;
    }

    public void TestUpdate()
    {
        if (Input.GetKeyDown(KeyCode.A))
            ToAttack();
        if (Input.GetKeyDown(KeyCode.B))
            ToBehaviour();
    }

    private void OnDie()
    {
        if (AttackBehaviour.IsActive)
            AttackBehaviour.DeActivate();
        if (Behaviour.IsActive)
            Behaviour.DeActivate();
    }

    public void ToAttack()
    {
        if (AttackBehaviour.IsActive) return;

        Behaviour.DeActivate();
        AttackBehaviour.Activate();

        _enemyVision.SetRadius(_visionRadiusInAttack * 2);
        _pointer.Activate();

        //_lineToPlayer.Activate();
    }

    public void ToBehaviour()
    {
        if (Behaviour.IsActive) return;

        if (AttackBehaviour.CurentPattern == _attackPattern)
        {
            #region *Coment about important bug*
            /* 
             * Это не очень то надежно. 
             * Имейте ввиду что это работает только если наша подписка последняя. Если после нашей подписки есть еще какая-
             * нибудь меняющая паттерн в AttackBehaviour, то после того как мы его выключим, он опять включится
             */
            #endregion

            DelayToBehaviourTransition();
            return;
        }

        AttackBehaviour.DeActivate();
        Behaviour.Activate();

        _enemyVision.SetRadius(_visionRadiusInIdle * 2);
        _pointer.Deactivate();

        //_lineToPlayer.Deactive();
    }

    private void DelayToBehaviourTransition()
    {
        IStateMachinePattern pattern = AttackBehaviour.CurentPattern;
        pattern.EndWorkEvent += Unsubscribe;

        void Unsubscribe()
        {
            pattern.EndWorkEvent -= Unsubscribe;
            HandleVision(_enemyVision.PlayerIsVision);
        }
    }

    private void HandleVision(bool visionStatus)
    {
        if (visionStatus)
        {
            if (AttackBehaviour.IsActive) return;
            ToAttack();
            return;
        }
        if (Behaviour.IsActive) return;
        ToBehaviour();
    }

    private void CreateAttackPattern()
    {
        AttackBehaviour = new StateMachineLevle();
        PersonMover mover = Person.Mover as PersonMover;

        _startAttackPattern.Transition.Add(new SimpleTransition(_goToOpponent, _startAttackPattern, AttackBehaviour));

        _attackPattern.Transition.Add(new SimpleTransition(_waitPattern, _attackPattern, AttackBehaviour));
        _attackPattern.PersonMover = mover;
        _attackPattern.Rotator = mover.Rotator;

        _waitPattern.Transition.Add(new SimpleTransition(_goToOpponent, _waitPattern, AttackBehaviour));
        _waitPattern.Mover = mover;

        _goToOpponent.Transition.Add(new SimpleTransition(_attackPattern, _goToOpponent, AttackBehaviour));
        _goToOpponent.PersonMover = mover;
        _goToOpponent.Rotator = mover.Rotator;


        AttackBehaviour.Init(_startAttackPattern, _attackPattern, _waitPattern, _goToOpponent, _startAttackPattern);
    }
}