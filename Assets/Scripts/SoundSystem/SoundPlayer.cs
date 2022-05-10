using Cephei;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SoundPlayer : MonoBehaviour
{
    public SoundBank Bank;

    private LinkedList<UsedSource> _sources;

    private void Update()
    {
        if (_sources == null)
            _sources = new LinkedList<UsedSource>();

        foreach (var setup in Bank.GetAllSounds())
        {
            if (setup.Play && setup.Clip != null)
            {
                setup.Play = false;

                GameObject newObject = new GameObject();
                newObject.transform.parent = transform;
                AudioSource source = newObject.AddComponent<AudioSource>();

                Sound.SetupSource(setup, source);
                source.Play();

                _sources.AddLast(new UsedSource(source, setup.Clip.length));
            }
        }

        LinkedListNode<UsedSource> curentNode = _sources.First;
        while (curentNode != null)
        {
            LinkedListNode<UsedSource> nextNode = curentNode.Next;

            if (curentNode.Value.TryDestroy(Time.deltaTime))
                _sources.Remove(curentNode);

            curentNode = nextNode;
        }
    }  
    
    private class UsedSource
    {
        public AudioSource Source;

        public float TimeToDestroy;
        public float Timer;

        public UsedSource(AudioSource source, float timeToDestroy)
        {
            Source = source;
            TimeToDestroy = timeToDestroy;
        }

        public bool TryDestroy(float deltaTime)
        {
            Timer += deltaTime;

            if (Timer > TimeToDestroy)
            {
                DestroyImmediate(Source.gameObject);
                return true;
            }
            
            return false;
        }
    }
}
