using Cephei;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class SlowMotionPitchChanger
{
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private AudioMixer _mixer;

    private float _startPitch;
    private float _pitchBefore;
    private bool _isFinish;
    private bool _isStart;

    public void Init()
    {
        _mixer.GetFloat("Pitch", out _startPitch);
    }

    public IEnumerator BeginSlowMotion(float duration)
    {
        float timer = default;
        SetStarting(true);

        //CustomDebug.Break();

        if (_mixer.GetFloat("Pitch", out _pitchBefore) == false)
            Debug.LogError("Mixer haven't 'Pitch' parrametr");

        while (timer < duration)
        {
            if (_isFinish)
                yield break;

            float pitch = _curve.Evaluate(timer / duration) * _pitchBefore;
            _mixer.SetFloat("Pitch", pitch);

            Debug.Log("Change pitch: " + pitch + "timer: " + timer);

            timer += Time.unscaledDeltaTime;

            yield return null;
        }

        _mixer.SetFloat("Pitch", _pitchBefore);
    }

    public IEnumerator EndSlowMotion(float timeToDefrosting)
    {
        float timer = default;
        SetStarting(false);

        _mixer.GetFloat("Pitch", out float pitchBeforeNormalize);

        while (timer < timeToDefrosting)
        {
            if (_isStart)
                yield break;

            float pitch = Mathf.Lerp(pitchBeforeNormalize, _startPitch, timer / timeToDefrosting);
            _mixer.SetFloat("Pitch", pitch);

            Debug.Log("Normalize pitch: " + pitch);

            timer += Time.unscaledDeltaTime;

            yield return null;
        }        

        _mixer.SetFloat("Pitch", _pitchBefore);
    }

    private void SetStarting(bool starting)
    {
        _isStart = starting;
        _isFinish = !starting;
    }
}
