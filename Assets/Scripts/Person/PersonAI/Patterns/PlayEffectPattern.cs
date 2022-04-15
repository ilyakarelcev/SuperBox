using Cephei;
using Cephei.StateMachine;
using UnityEngine;

[System.Serializable]
public class PlayEffectPattern : StateMachinePatternBase
{
    [SerializeField] private AudioSource _source;

    public override void Activate()
    {
        base.Activate();

        _source.Play();
        InvokeEndWorkEvent();
    }
}
