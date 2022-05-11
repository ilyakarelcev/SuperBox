using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDownOnEnemyKilled : MonoBehaviour
{
    [SerializeField] private CurveParrametrs _toDown;
    [SerializeField] private CurveParrametrs _toUp;
    [Space]
    [SerializeField] private float _distance;
    [SerializeField] private float _time;
    [SerializeField] private AnimationCurve _curve;
    [Space]
    [SerializeField] private BoxAttacker _boxAttacker;
    [SerializeField] private CameraHeightAnimator _cameraHeightAnimator;

    private bool _isWork;
    
    private void Start()
    {
        _boxAttacker.AttackEvent += OnAttack;
    }    

    private void OnAttack(Attack attack)
    {
        attack.AttackedPerson.HealthManager.DieEvent += PlayEffect;
        attack.EndAttackEvent += 
            () => attack.AttackedPerson.HealthManager.DieEvent -= PlayEffect;
    }

    private void PlayEffect()
    {
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
}
