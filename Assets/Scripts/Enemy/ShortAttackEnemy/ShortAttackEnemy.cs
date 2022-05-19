using UnityEngine;

public class ShortAttackEnemy : PersonBase
{
    [SerializeField] private ShortAttackEnemyAI AI;
    [Header("Effects")]
    [SerializeField] private CollisionEfect _collisionParticlsEfect;
    [SerializeField] private CollisionEfect _collisionParticlsEfectDontBreakAttack;
    [SerializeField] private CircleEffectForDontBreakAttack _circleEffectForAttack;
    [Header("Sound")]
    [SerializeField] private SoundAttackTaker _soundAttackTaker;
    [SerializeField] private SoundAttackTaker _soundOnDontBreakAttack;
    [SerializeField] private SoundAttackViewSuport _soundAttackViewSuport;
    [Header("Attack")]
    [SerializeField] private DontBreakerAttackAnimationView _attackView;
    [SerializeField] private AttackInterpretator _coificentCalculator;
    [SerializeField] private DontBreakerAttackHandler _dontBreakerAttackHandler;
    [Space]
    [Space]
    [SerializeField] private HealthManager _healthManager;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private PersonMover _mover;

    private void Start()
    {
        AttackTakerManagerBase attackTaker = new AttackTakerManagerBase(_coificentCalculator);
        attackTaker.AttackTakers.Add(_healthManager);
        attackTaker.AttackTakers.Add(new ImpulsTaker());
        attackTaker.AttackTakers.Add(_collisionParticlsEfect);
        attackTaker.AttackTakers.Add(_soundAttackTaker);

        _dontBreakerAttackHandler.Init(
            new IAttackTaker[] { _collisionParticlsEfect, _soundAttackTaker },
            new IAttackTaker[] { _collisionParticlsEfectDontBreakAttack, _soundOnDontBreakAttack }
            );

        Init(_healthManager, _rb, _mover, attackTaker, transform);

        AddComponents(_mover, _attackView, _dontBreakerAttackHandler, AI); 

        InitializeAllComponent();
        InitializeThisComponents(_soundAttackViewSuport, _collisionParticlsEfect, _collisionParticlsEfectDontBreakAttack);

        InitSound();

        //Init CircleEffect if you need use it
    }

    private void InitSound()
    {
        _soundAttackTaker.Init(Sound.Bank.Hit);
        _soundOnDontBreakAttack.Init(Sound.Bank.HitOnDontBreakState);

        _soundAttackViewSuport.Init(Sound.Bank.SwordAttack);
    }
}