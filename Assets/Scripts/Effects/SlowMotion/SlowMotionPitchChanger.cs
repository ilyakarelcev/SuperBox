using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SlowMotionPitchChanger : MonoBehaviour
{
    [SerializeField] private AnimationCurve _pitchOnTimeCurve;
    [Space]
    [SerializeField] private AudioMixer _mixer;

    private SlowMotionManager _manager;

    [Range(0, 1)]
    public float TestScale;

    private void Start()
    {
        _manager = SlowMotionManager.Instance;

        _manager.BeginWorkEvent += OnBeginWork;
        _manager.EndWorkEvent += OnEndWork;

        enabled = false;
    }

    private void Update()
    {
        //_mixer.SetFloat("Pitch", _pitchOnTimeCurve.Evaluate(TestScale));// Test

        _mixer.SetFloat("Pitch", _pitchOnTimeCurve.Evaluate(_manager.CurrentScale));
    }

    public void OnBeginWork()
    {
        enabled = true;
    }

    public void OnEndWork()
    {
        enabled = false;
    }
}
