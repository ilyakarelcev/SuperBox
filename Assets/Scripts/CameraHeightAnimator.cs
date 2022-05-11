using Cephei;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//—крипт не умеет работать с отрицательными значени€ми локальной позиции. ѕрименительно к камере это значит что нельз€ отдавать скрипту дельту, котора€ уведет камеру ниже локального 0

public class CameraHeightAnimator : MonoBehaviour
{
    public Transform _camera;

    public bool IsFreez { get; private set; }

    public Action StartAnimationEvent;
    public Action EndAnimationEvent;

    private Action _freezingOperation;
    private float _startDistance;
    private Vector3 _normalizePosition;

    private float Delta
    {
        get => _camera.localPosition.magnitude - _startDistance;
        set => _camera.localPosition = _normalizePosition * (_startDistance + value);
    }

    [Space]
    public float TestDelta;
    public bool TestByCurve;
    public float TestTime;
    public AnimationCurve Curve;

    private void Start()
    {
        _startDistance = _camera.localPosition.magnitude;//
        _normalizePosition = _camera.localPosition.normalized;
    }

    private void Update()
    {
        if (TestByCurve)
        {
            TestByCurve = false;
            SetAnimation(TestDelta, TestTime, Curve);
        }
    }

    [ContextMenu("Freez")]
    public void Freeze()
    {
        IsFreez = true;
    }

    [ContextMenu("UnFreez")]
    public void UnFreeze()
    {
        IsFreez = false;
        StopAllCoroutines(); //¬озможно стоит подписатьс€ на окончание этой анимации и запустить замороженную анимацию

        _freezingOperation?.Invoke();
        _freezingOperation = null;
    }    

    public void SetAnimation(float delta, float time, AnimationCurve curve)
    {
        Action action = () => { StartCoroutine(Animate(delta, time, curve)); };

        SetAnimation(action);
    }

    public void SetAnimation(float delta, float speed)
    {
        Action action = () => { StartCoroutine(Animate(delta, speed)); };

        SetAnimation(action);
    }

    private void SetAnimation(Action action)
    {
        if (IsFreez)
            _freezingOperation = action;
        else
        {
            StopAllCoroutines();
            action.Invoke();
        }
    }

    private IEnumerator Animate(float delta, float time, AnimationCurve curve)
    {
        StartAnimationEvent?.Invoke();

        float timer = 0;
        float beforeAnimateDelta = Delta;

        while (timer < time)
        {
            float percent = timer / time;
            Delta = Cephei.Math.LerpByCurve(beforeAnimateDelta, delta, curve, percent);

            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        Delta = delta;
        EndAnimationEvent?.Invoke();
    }

    private IEnumerator Animate(float delta, float speed)
    {
        StartAnimationEvent?.Invoke();

        float currentDelta = Delta;
        float sign = (delta - currentDelta).Sign();

        while (true)
        {
            float currentSpeed = speed * Time.unscaledDeltaTime;
            currentDelta += sign * currentSpeed;

            if ((delta - currentDelta).Abs() < currentSpeed)
                break;

            Delta = currentDelta;

            yield return null;
        }

        Delta = delta;
        EndAnimationEvent?.Invoke();
    }
}

[System.Serializable]
public struct CurveParrametrs
{
    public float Magnitude;
    public float Time;
    public AnimationCurve Curve;
}
