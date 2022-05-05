using System;
using UnityEngine;

[Serializable]
public class SoundSetup
{
    [Range(0, 1)]
    [SerializeField] private float _volume = 1;
    [Range(0, 3)]
    [SerializeField] private float _pitch = 1;
    [Space]
    [SerializeField] private AudioClip _clip;    

    public AudioClip Clip => _clip;
    public float Volume => _volume;
    public float Pitch => _pitch;

    public SoundSetup(float volume, float pitch, AudioClip clip)
    {
        _volume = volume;
        _pitch = pitch;
        _clip = clip;
    }
}
