using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionSound : MonoBehaviour
{
    [SerializeField] private AnimationCurve _volumeOnScaleCurve;
    [Space]
    [SerializeField] private AudioSource _source;

    private SlowMotionManager _manager;

    [Range(0, 1)]
    public float TestScale;

    private void Start()
    {
        _manager = SlowMotionManager.Instance;

        _manager.BeginWorkEvent += OnBeginWork;
        _manager.EndWorkEvent += OnEndWork;
    }

    private void Update()
    {
        //_source.volume = _volumeOnScaleCurve.Evaluate(TestScale);//Test

        _source.volume = _volumeOnScaleCurve.Evaluate(_manager.CurrentScale);
    }

    public void OnBeginWork()
    {
        _source.Play();

        enabled = true;
    }

    public void OnEndWork()
    {
        _source.Stop();

        enabled = false;
    }
}