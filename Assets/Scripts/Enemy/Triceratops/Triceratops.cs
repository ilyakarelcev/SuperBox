using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triceratops : PersonBase
{
    [SerializeField] private TriceratopsAI AI;
    [Header("Effects")]
    [SerializeField] private CollisionEfect _collisionParticlsEfect;
    [SerializeField] private CircleEffect _circleEffectForAttack;//// Init this components
    [SerializeField] private EffectOnBeginOfDamageAttack _attackEffect;//// Init this components
    [Header("Sound")]
    [SerializeField] private SoundAttackTaker _soundAttackTaker;
    [SerializeField] private SoundAttackViewSuport _soundAttackViewSuport;
    [Header("Attack")]
    [SerializeField] private TriceratopsAttackView _attackView;
    [SerializeField] private AttackCoificentCalculator _coificentCalculator;
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

        AddComponents(_mover, _attackView, AI);

        InitializeAllComponent();
        InitializeThisComponents(_soundAttackViewSuport, _collisionParticlsEfect, _soundAttackTaker);
    }
}
