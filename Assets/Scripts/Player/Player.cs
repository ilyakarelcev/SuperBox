using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PersonBase
{
    [Header("Box")]
    [SerializeField] private BoxPushAway _boxPushAway;
    [SerializeField] private JumpInfoConteiner JumpInfoConteiner;
    [Header("Effects")]
    [SerializeField] private CollisionEfect _collisionParticlsEfect;
    [SerializeField] private CameraShakeOnHandleAttack _shakeOnHandleAttack;
    [SerializeField] private CameraShakeOnTakeAttack _shakeOnTakeAttack;
    [Header("Sound")]
    [SerializeField] private SoundAttackTaker _soundAttackTaker;
    [SerializeField] private SoundAttackHandler _soundAttackHandler;
    [Header("Attack")]
    [SerializeField] private AttackCoificentCalculator _coificentCalculator;
    [Space]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private HealthManager _healthManager;

    private void Start()
    {
        AttackTakerManagerBase attackTakerManager = new AttackTakerManagerBase(_coificentCalculator);
        attackTakerManager.AttackTakers.Add(_healthManager);
        attackTakerManager.AttackTakers.Add(new ImpulsTaker());
        attackTakerManager.AttackTakers.Add(_collisionParticlsEfect);
        attackTakerManager.AttackTakers.Add(_shakeOnTakeAttack);
        attackTakerManager.AttackTakers.Add(_soundAttackTaker);

        Init(_healthManager, _rigidbody, new NullMover(), attackTakerManager, transform, 
            _boxPushAway,
            JumpInfoConteiner,
            _shakeOnHandleAttack,
            _soundAttackHandler,
            _soundAttackTaker,
            _collisionParticlsEfect);

        InitializeAllComponent();
    }
}