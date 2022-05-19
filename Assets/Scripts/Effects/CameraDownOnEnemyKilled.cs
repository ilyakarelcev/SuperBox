using Cephei;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDownOnEnemyKilled : MonoBehaviour
{
    [SerializeField, Range(0, 1)] float _chance = 1f;
    [SerializeField, Range(0,1)] private float _minAttackMultiply = 0.8f;
    [SerializeField] private CurveParrametrs _toDown;
    [SerializeField] private CurveParrametrs _toUp;
    [Space]
    [SerializeField] private CurveParrametrs _timeCurve;
    [Space]
    [SerializeField] private BoxAttacker _boxAttacker;
    [SerializeField] private CameraHeightAnimator _cameraHeightAnimator;

    private bool _isWork;

    SmartRandom smartRandom;

    private void Start()
    {
        smartRandom = new SmartRandom(5, 5);

        _boxAttacker.AttackEvent += OnAttack;
    }    

    private bool CalculateChance(float multiply)
    {
        // Эта формула уменьшает вероятность срабатывания пропорционально выличине удара бокса        
        float attackPercent = multiply / _minAttackMultiply;
        float randomValue = smartRandom.GetValue();
        return _chance * attackPercent.Sqr(3) >= randomValue;
    }

    private void OnAttack(Attack attack)
    {
        attack.AttackedPerson.HealthManager.DieEvent += TryPlay;
        attack.EndAttackEvent += 
            () => attack.AttackedPerson.HealthManager.DieEvent -= TryPlay;

        void TryPlay()
        {
            if (attack.AttackMultiply > _minAttackMultiply || CalculateChance(attack.AttackMultiply))
                PlayEffect();
        }
    }

    private void PlayEffect()
    {
        HandleTime();

        if (_isWork)
        {
            _cameraHeightAnimator.EndAnimationEvent -= SecondPart;
            _cameraHeightAnimator.EndAnimationEvent -= UnFreeze;
            _cameraHeightAnimator.UnFreeze();
        }

        _isWork = true;

        _cameraHeightAnimator.SetAnimation(-1 * _toDown.Magnitude, _toDown.Time, _toDown.Curve);
        _cameraHeightAnimator.Freeze();

        _cameraHeightAnimator.EndAnimationEvent += SecondPart;

        void SecondPart()
        {
            _cameraHeightAnimator.UnFreeze();
            _cameraHeightAnimator.SetAnimation(_toUp.Magnitude, _toUp.Time, _toUp.Curve);
            _cameraHeightAnimator.Freeze();

            _cameraHeightAnimator.EndAnimationEvent -= SecondPart;
            _cameraHeightAnimator.EndAnimationEvent += UnFreeze;
        }

        void UnFreeze()
        {
            _cameraHeightAnimator.EndAnimationEvent -= UnFreeze;
            _isWork = false;

            _cameraHeightAnimator.UnFreeze();
        }
    }

    private void HandleTime()
    {
        SlowMotionManager.Instance.AddOperation(_timeCurve.Time, (p) => _timeCurve.Curve.Evaluate(p) * _timeCurve.Magnitude);
    }
}
