using Cephei;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSlowMotion : MonoBehaviour
{
    [SerializeField] private float _duration = 3;
    [SerializeField] private float _timeToDefrosting = 0.5f;
    [SerializeField] private float _lowTimeScale = 0.4f;
    [Space]
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private AnimationCurve _soundCurve;
    [Space]
    [SerializeField] private SlowMotionPitchChanger _pitchChanger;
    [Space]
    [SerializeField] private AudioSource _sound;
    [SerializeField] private Joystick _joystick;

    private Coroutine _audioCoroutine;
    private float _startVolume;

    [Space]
    [SerializeField] private SlowMotionDebug _debug;

    public float TimerView;

    private void Start()
    {
        Init(_joystick);        
    }

    private void Update()
    {
        _debug.CurentScale = Time.timeScale;
    }

    public void Init(Joystick joystick)
    {
        _joystick = joystick;

        _joystick.OnDownEvent += StartSlowMotion;
        _joystick.OnUpEvent += FinishSlowMotion;

        _startVolume = _sound.volume;

        _pitchChanger.Init();
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

    public void StartSlowMotion(Vector2 vector2)
    {
        TimeScaleManager.LerpByCurve(_curve, _duration);

        if (_audioCoroutine != null)
            StopCoroutine(_audioCoroutine);
        _audioCoroutine =  StartCoroutine(ChangeVolumeByCurve());

        StartCoroutine(_pitchChanger.BeginSlowMotion(_duration));
    }
    
    public void FinishSlowMotion(Vector2 vector2)
    {
        TimeScaleManager.LerpTo(1, _timeToDefrosting);

        if (_audioCoroutine != null)
            StopCoroutine(_audioCoroutine);
        _audioCoroutine = StartCoroutine(NormalizeVolume());

        StartCoroutine(_pitchChanger.EndSlowMotion(_timeToDefrosting));
    }

    private IEnumerator ChangeVolumeByCurve()
    {
        _sound.Play();
        float timer = default;

        while (timer < _duration)
        {
            _sound.volume = _startVolume * _soundCurve.Evaluate(timer / _duration);

            timer += Time.unscaledDeltaTime;

            _debug.Timer = timer;

            yield return null;
        }

        _sound.volume = 0;        
    }

    private IEnumerator NormalizeVolume()
    {
        float timer = default;
        float volume = _sound.volume;

        while (timer < _timeToDefrosting)
        {
            _sound.volume = Mathf.Lerp(volume, 0, timer / _timeToDefrosting);

            timer += Time.unscaledDeltaTime;

            _debug.TimerFotDefrosting = timer;

            yield return null;
        }
        _sound.volume = 0;
    }

    [System.Serializable]
    private class SlowMotionDebug
    {
        public float CurentScale;
        public float Timer;
        public float TimerFotDefrosting;
    }
}
