﻿using UnityEngine;

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
    [SerializeField] private AttackCoificentCalculator _coificentCalculator;
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

        Init(_healthManager, _rb, _mover, attackTaker, transform, 
            _mover, 
            _attackView,
            _circleEffectForAttack,
            _collisionParticlsEfect,
            _collisionParticlsEfectDontBreakAttack,
            _dontBreakerAttackHandler,
            _soundAttackViewSuport,
            _soundAttackTaker,
            _soundOnDontBreakAttack,
            AI);

        InitializeAllComponent();
    }
}