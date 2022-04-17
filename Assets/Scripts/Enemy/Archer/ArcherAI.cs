using Cephei.StateMachine;
using System;
using UnityEngine;

[System.Serializable]
public class ArcherAI : IPersonComponent
{
    [SerializeField] private float _visionRadiusInIdle = 20;
    [SerializeField] private float _visionRadiusInAttack = 30;
    [Space]
    [SerializeField] private BehaviourSelector _behaviourSelector;

    [Header("AttackPattern")]
    [SerializeField] private ArcherAttackPattern _archerAttackPattern;
    [SerializeField] private StanPattern _stanPattern;
    [SerializeField] private RotateToOpponent _rotateToOpponent;
    [SerializeField] private StopAndWaitPattern _waitPattern;
    [SerializeField] private ArcherStartAttackPatternWithDelay _archerStartPattern;
    [Space]
    [SerializeField] private ToStanTransition _toStanTransition;

    public IPerson Person { get; private set; }

    public StateMachineLevle AttackBehaviour { get; private set; }
    public StateMachineLevle Behaviour { get; private set; }

    private EnemyVision _enemyVision;
    private LineToPlayerOnInvisible _lineToPlayer;

    [Space]
    public bool KeyboardTesting;

    public void Init(IPerson person)
    {
        Person = person;

        _enemyVision = person.GetPersonComponent<EnemyVision>();
        _enemyVision.ChangeIsPlayerVisionEvent += HandleVision;

        _lineToPlayer = person.GetPersonComponent<LineToPlayerOnInvisible>();

        CreateAttackPattern();
        Behaviour = _behaviourSelector.GetBehaviour();

        Person.InitializeThisComponents(
            _archerStartPattern, _archerAttackPattern, _stanPattern, _rotateToOpponent, _waitPattern, Behaviour as IPersonComponent);

        Behaviour.Activate();
        _enemyVision.SetRadius(_visionRadiusInIdle * 2);


        Person.Operator.OpenUpdateCoroutine(TestUpdate, LifeType.Cycle);//Test
    }

    public void TestUpdate()
    {
        if(KeyboardTesting == false)
            return;

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

        if (AttackBehaviour.CurentPattern == _stanPattern || AttackBehaviour.CurentPattern == _archerAttackPattern)
        {
            #region *Coment about important bug*
            /* 
             * Ёто не очень то надежно. 
             * »мейте ввиду что это работает только если наша подписка последн€€. ≈сли после нашей подписки есть еще кака€-
             * нибудь мен€юща€ паттерн в AttackBehaviour, то после того как мы его выключим, он оп€ть включитс€
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
        Rotator mover = Person.Mover as Rotator;

        _archerStartPattern.Transition.Add(new SimpleTransition(_rotateToOpponent, _archerStartPattern, AttackBehaviour));
        _archerStartPattern.Mover = mover;

        _archerAttackPattern.Transition.Add(new SimpleTransition(_waitPattern, _archerAttackPattern, AttackBehaviour));
        _archerAttackPattern.Mover = mover;

        _waitPattern.Transition.Add(new SimpleTransition(_rotateToOpponent, _waitPattern, AttackBehaviour));
        _waitPattern.Mover = mover;

        _rotateToOpponent.Transition.Add(new SimpleTransition(_archerAttackPattern, _rotateToOpponent, AttackBehaviour));
        _rotateToOpponent.Mover = mover;

        _stanPattern.Transition.Add(new SimpleTransition(_waitPattern, _stanPattern, AttackBehaviour));
        _stanPattern.Mover = mover;


        _archerAttackPattern.Transition.Add(_toStanTransition);
        _rotateToOpponent.Transition.Add(_toStanTransition);
        _waitPattern.Transition.Add(_toStanTransition);


        AttackBehaviour.Init(_archerStartPattern, _archerAttackPattern, _waitPattern, _stanPattern, _rotateToOpponent, _archerStartPattern);
        _toStanTransition.Init(_stanPattern, AttackBehaviour, Person.HealthManager, Person.AttackTakerManager);
    }
}