using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Sound : ISengleTone
{
    [SerializeField] private SoundBank _bank;

    public static SoundBank Bank { get; private set; }

    private static LinkedList<AudioSource> _sources;

    public void Init()
    {
        Bank = _bank;
        _sources = new LinkedList<AudioSource>();
    }

    public static void SetupSource(SoundSetup setup, AudioSource source)
    {
        source.clip = setup.Clip;
        source.volume = setup.Volume;
        source.pitch = setup.Pitch;
    }

    public static void AddSource(AudioSource source)
    {
        _sources.AddLast(source);
    }
}
