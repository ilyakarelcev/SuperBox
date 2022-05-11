using Cephei;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUpOnJoysticMove : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float _trashHold = 0.7f;
    [Space]
    [SerializeField] private CurveParrametrs _toDown;
    [SerializeField] private CurveParrametrs _toUp;
    [Space]
    [SerializeField] private Joystick _joystick;
    [SerializeField] private CameraHeightAnimator _cameraHeightAnimator;

    private float _lastFrameValue;

    private void Start()
    {
        _joystick.OnDownEvent += OnDown;
        _joystick.OnUpEvent += OnUp;
    }

    private void OnDown(Vector2 vector2)
    {
        _cameraHeightAnimator.SetAnimation(_toDown.Magnitude, _toDown.Time, _toDown.Curve);
    }

    private void OnUp(Vector2 vector2)
    {
        _cameraHeightAnimator.SetAnimation(_toUp.Magnitude, _toUp.Time, _toUp.Curve);
    }
}