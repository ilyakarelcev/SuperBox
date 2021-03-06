using UnityEngine;

public class Archer : PersonBase
{
    [SerializeField] private ArcherAI AI;
    [Header("Effect")]
    [SerializeField] private CollisionEfect _collisionParticlsEfect;
    [SerializeField] private CircleEffect _circleEffect;
    [Header("Sound")]
    [SerializeField] private SoundAttackTaker _soundAttackTaker;
    [SerializeField] private SoundAttackViewSuport _soundAttackViewSuport;
    [Header("Attack")]
    [SerializeField] private AttackAnimationView _attackAnimationController;
    [SerializeField] private AttackInterpretator _coificentCalculator;
    [Space]
    [SerializeField] private HealthManager _healthManager;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Rotator _mover;

    private void Start()
    {
        AttackTakerManagerBase attackTaker = new AttackTakerManagerBase(_coificentCalculator);
        attackTaker.AttackTakers.Add(new ImpulsTaker());
        attackTaker.AttackTakers.Add(_healthManager);
        attackTaker.AttackTakers.Add(_collisionParticlsEfect);
        attackTaker.AttackTakers.Add(_soundAttackTaker);

        Init(_healthManager, _rb, _mover, attackTaker, transform);

        AddComponents(_attackAnimationController, _mover, AI);
        InitializeAllComponent();

        InitializeThisComponents(_collisionParticlsEfect, _soundAttackViewSuport);

        InitSound();

        //Init circle effect if you need use it
    }

    private void InitSound()
    {
        _soundAttackTaker.Init(Sound.Bank.Hit);
        _soundAttackViewSuport.Init(Sound.Bank.Shoot);
    }
}