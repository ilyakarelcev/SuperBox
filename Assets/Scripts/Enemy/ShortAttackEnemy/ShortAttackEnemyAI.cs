using Cephei.StateMachine;
using UnityEngine;

[System.Serializable]
public class ShortAttackEnemyAI : IPersonComponent
{
    [SerializeField] private float _visionRadiusInIdle = 20;
    [SerializeField] private float _visionRadiusInAttack = 30;
    [Space]
    [SerializeField] private BehaviourSelector _behaviourSelector;

    [Header("AttackPattern")]
    [SerializeField] private ShortDontBreakerAttackPattern _shortAttackPattern;
    [SerializeField] private StanPattern _stanPattern;
    [SerializeField] private GoToOpponentPattern _goToOpponent;
    [SerializeField] private WaitAfterAttackPattern _waitPattern;
    [SerializeField] private PlayEffectPattern _startAttackPattern;
    [Space]
    [SerializeField] private ToStanTransition _toStanTransition;

    public IPerson Person { get; private set; }

    public StateMachineLevle AttackBehaviour  { get; private set; }
    public StateMachineLevle Behaviour { get; private set; }

    private EnemyVision _enemyVision;
    private LineToPlayerOnInvisible _lineToPlayer;

    public void Init(IPerson person)
    {
        Person = person;

        _enemyVision = Person.GetPersonComponent<EnemyVision>();
        _enemyVision.ChangeIsPlayerVisionEvent += HandleVision;

        _lineToPlayer = person.GetPersonComponent<LineToPlayerOnInvisible>();

        CreateAttackPattern();
        Behaviour = _behaviourSelector.GetBehaviour();
        (Behaviour as IPersonComponent).Init(person);        

        Person.InitializeThisComponents(
            _shortAttackPattern, _stanPattern, _goToOpponent, _waitPattern, Behaviour as IPersonComponent);

        Behaviour.Activate();

        Person.Operator.OpenUpdateCoroutine(TestUpdate, LifeType.Cycle);//Test
    }

    public void TestUpdate()
    {
        if (Input.GetKeyDown(KeyCode.A))
            ToAttack();
        if (Input.GetKeyDown(KeyCode.B))
            ToBehaviour();
    }

    public void ToAttack()
    {
        if (AttackBehaviour.IsActive) return;

        Behaviour.DeActivate();
        AttackBehaviour.Activate();

        _enemyVision.SetRadius(_visionRadiusInAttack * 2);
        _lineToPlayer.Activate();
    }

    public void ToBehaviour()
    {
        if (Behaviour.IsActive) return;

        if (AttackBehaviour.CurentPattern == _stanPattern || AttackBehaviour.CurentPattern == _shortAttackPattern)
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
        _lineToPlayer.Deactive();
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

        _shortAttackPattern.Transition.Add(new SimpleTransition(_waitPattern, _shortAttackPattern, AttackBehaviour));
        _shortAttackPattern.Mover = mover;

        _waitPattern.Transition.Add(new SimpleTransition(_goToOpponent, _waitPattern, AttackBehaviour));
        _waitPattern.Mover = mover;
        _waitPattern.Rotator = mover.Rotator;

        _goToOpponent.Transition.Add(new SimpleTransition(_shortAttackPattern, _goToOpponent, AttackBehaviour));
        _goToOpponent.Mover = mover;

        _stanPattern.Transition.Add(new SimpleTransition(_waitPattern, _stanPattern, AttackBehaviour));
        _stanPattern.Mover = mover;


        _shortAttackPattern.Transition.Add(_toStanTransition);
        _goToOpponent.Transition.Add(_toStanTransition);
        _waitPattern.Transition.Add(_toStanTransition);


        AttackBehaviour.Init(_startAttackPattern,  _shortAttackPattern, _waitPattern, _stanPattern, _goToOpponent, _startAttackPattern);
        _toStanTransition.Init(_stanPattern, AttackBehaviour, Person.HealthManager, Person.AttackTakerManager);
    }
}