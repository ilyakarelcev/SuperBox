using Cephei;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUpOnJoysticMove : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float _trashHold = 0.7f;
    [SerializeField] private float _speed = 5;
    [Space]
    [SerializeField] private float _distance = 2;
    [SerializeField] private float _timeToUp = 1;///
    [SerializeField] private float _timeBack = 1;
    [SerializeField] private AnimationCurve _curve;///
    [Space]
    [SerializeField] private Joystick _joystick;
    [SerializeField] private CameraHeightAnimator _cameraHeightAnimator;

    private float _lastFrameValue;

    private void Start()
    {
        _joystick.OnPressedEvent += OnPressed;
        _joystick.OnUpEvent += OnUp;
    }

    private void OnPressed(Vector2 vector2)
    {
        float value = _joystick.Value.magnitude;
        if ((value < _trashHold && _lastFrameValue < _trashHold) || value == _lastFrameValue)
            return;

        float percent = Mathf.InverseLerp(_trashHold, 1, value);
        _cameraHeightAnimator.SetAnimation(_distance * percent, _speed);
    }

    private void OnUp(Vector2 vector2)
    {
        _cameraHeightAnimator.SetAnimation(0, _speed);
    }
}