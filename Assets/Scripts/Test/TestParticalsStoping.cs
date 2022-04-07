using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestParticalsStoping : MonoBehaviour
{
    public ParticleSystem ParticleSystem;

    public bool Play;
    public bool Stop;

    void Update()
    {
        if (Play)
            ParticleSystem.Play();
        if (Stop)
            ParticleSystem.Stop();

        Play = false;
        Stop = false;
    }
}
