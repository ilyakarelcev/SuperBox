using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleManager : MonoBehaviour, ISengleTone
{
    [SerializeField] private float _startTimeScale;

    private enum TypeMove
    {
        Linearly,
        Curve
    }

    private static float _fromTimeScale;
    private static float _targetTimeScale;
    private static float _timeToTarget;

    private static float _delta;
    private static AnimationCurve _lerpCurve;
    public static bool TargetTimeAchieved { get; private set; }

    private static bool Instance;
    private static float _fixedTimeStep;

    private static TypeMove _typeMove;

    public void Init()
    {
        if (Instance) Debug.Log("More one instance");
        Instance = true;

        _fixedTimeStep = Time.fixedDeltaTime;

        _delta = 1;
        _targetTimeScale = Time.timeScale;

        SetTimeScale(_startTimeScale);
    }

    private void Update() // Then change on Runtime operation
    {
        if (_delta >= 1)
        {
            TargetTimeAchieved = true;
            return;
        }

        _delta += Time.unscaledDeltaTime / _timeToTarget;

        if (_typeMove == TypeMove.Linearly)
            SetTimeScaleAndFixedStep(Mathf.Lerp(_fromTimeScale, _targetTimeScale, _delta));

        else if (_typeMove == TypeMove.Curve)
            SetTimeScaleAndFixedStep(_lerpCurve.Evaluate(_delta));
    }

    public static void NormalizeTimeScale(float time)
    {
        LerpTo(1, time);
    }

    public static void LerpTo(float targetTimeScale, float time)
    {
        _typeMove = TypeMove.Linearly;

        if (time <= 0) SetTimeScale(targetTimeScale);

        _fromTimeScale = Time.timeScale;
        _targetTimeScale = targetTimeScale;
        _timeToTarget = time;

        TargetTimeAchieved = false;
        _delta = 0;
    }

    public static void LerpByCurve(AnimationCurve curve, float time)
    {
        _typeMove = TypeMove.Curve;

        if (time <= 0) SetTimeScale(curve.Evaluate(1));

        _lerpCurve = curve;
        _timeToTarget = time;

        TargetTimeAchieved = false;
        _delta = 0;
    }

    public static void SetTimeScale(float timeScale)
    {
        SetTimeScaleAndFixedStep(timeScale);
        _delta = 1;
    }

    private static void SetTimeScaleAndFixedStep(float timeScale)
    {
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = _fixedTimeStep * timeScale;
    }
}