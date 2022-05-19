using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PersonBase
{
    [Header("Box")]
    [SerializeField] private BoxPushAway _boxPushAway;
    private VelocitySaver _velocitySaver = new VelocitySaver();
    [SerializeField] private JumpInfoConteiner JumpInfoConteiner;
    [Header("Effects")]
    [SerializeField] private CollisionEfect _collisionParticlsEfect;
    [SerializeField] private CameraShakeOnHandleAttack _shakeOnHandleAttack;
    [SerializeField] private CameraShakeOnTakeAttack _shakeOnTakeAttack;
    [SerializeField] private HealthChangedScreen _healthScreen;
    [Header("Sound")]
    [SerializeField] private SoundAttackTaker _soundAttackTaker;
    [SerializeField] private SoundAttackHandler _soundAttackHandler;
    [Header("Attack")]
    [SerializeField] private AttackInterpretator _coificentCalculator;
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

        Init(_healthManager, _rigidbody, new NullMover(), attackTakerManager, transform);

        AddComponents(_boxPushAway, _velocitySaver, JumpInfoConteiner, _shakeOnHandleAttack, _soundAttackHandler);

        InitializeAllComponent();
        InitializeThisComponents(_collisionParticlsEfect, _healthScreen);

        InitSound();
    }

    private void InitSound()
    {
        _soundAttackHandler.Init(Sound.Bank.BoxAttack);

        _soundAttackTaker.Init(Sound.Bank.Hit);
    }
}