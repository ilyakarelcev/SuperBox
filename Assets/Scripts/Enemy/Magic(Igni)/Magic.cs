using UnityEngine;

public class Magic : PersonBase
{
    [SerializeField] private MagicAI AI;
    [Header("Effects")]
    [SerializeField] private CollisionEfect _collisionParticlsEfect;
    [SerializeField] private MagicCircleEffect _circleEffect;
    [SerializeField] private EffectOnBeginOfDamageAttack _attackEffect;
    [Header("Sound")]
    [SerializeField] private SoundAttackTaker _soundAttackTaker;
    [SerializeField] private SoundAttackViewSuport _soundAttackViewSuport;
    [Header("Attack")]
    [SerializeField] private MagicViewAttacker _magicViewAttacker;
    [SerializeField] private AttackInterpretator _coificentCalculator;
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

        Init(_healthManager, _rb, _mover, attackTaker, transform);

        AddComponents(_mover, _magicViewAttacker, AI, _circleEffect);

        InitializeAllComponent();
        InitializeThisComponents(_soundAttackViewSuport, _collisionParticlsEfect, _attackEffect);

        InitSound();
    }

    private void InitSound()
    {
        _soundAttackTaker.Init(Sound.Bank.Hit);
        _soundAttackViewSuport.Init(Sound.Bank.FireCast);
    }
}