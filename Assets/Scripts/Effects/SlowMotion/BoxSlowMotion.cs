using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSlowMotion : MonoBehaviour
{
    [SerializeField] private float _timeToDefrosting = 0.5f;
    [SerializeField] private CurveParrametrs _timeCurve;
    [Space]
    [SerializeField] private Joystick _joystick;

    private SlowMotionManager.Operation _timeOperation;

    [Space]
    [SerializeField] private SlowMotionDebug _debug;

    private void Start()
    {
        Init(_joystick);        
    }

    #region Debug/Test
    private void Update()
    {
        _debug.CurentScale = Time.timeScale;
    }

    [ContextMenu("StartSlowMotion")]
    public void StartSlowMotionInEditor()
    {
        StartSlowMotion(default);
    }

    [ContextMenu("EndSlowMotion")]
    public void EndSlowMotionInEditor()
    {
        FinishSlowMotion(default);
    }
    #endregion

    public void Init(Joystick joystick)
    {
        _joystick = joystick;

        _joystick.OnDownEvent += StartSlowMotion;
        _joystick.OnUpEvent += FinishSlowMotion;
    }

    public void StartSlowMotion(Vector2 vector2)
    {
        Func<float, float> func = (p) => _timeCurve.Curve.Evaluate(p) * _timeCurve.Magnitude;
        _timeOperation = SlowMotionManager.Instance.AddOperation(_timeCurve.Time, func);
    }
    
    public void FinishSlowMotion(Vector2 vector2)
    {
        SlowMotionManager.Instance.RemoveOperation(_timeOperation);

        Func<float, float> func = (p) => Mathf.Lerp(Time.timeScale, 1, p);
        SlowMotionManager.Instance.AddOperation(_timeToDefrosting, func);
    }

    [System.Serializable]
    private class SlowMotionDebug
    {
        public float CurentScale;
        public float Timer;
        public float TimerFotDefrosting;
    }
}
