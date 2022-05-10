using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class AudioSourcePlayer : MonoBehaviour
{
    public bool Play;

    public AudioSource _source;

    void Update()
    {
        if (Play || Input.GetKeyDown(KeyCode.P))
        {
            Play = false;

            _source.Play();
        }
    }
}
