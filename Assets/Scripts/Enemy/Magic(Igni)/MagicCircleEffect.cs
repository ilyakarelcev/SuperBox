using Cephei.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MagicCircleEffect
{
    [SerializeField] private ParticleSystem _effect;

    private bool _isActive;

    public void Init(StateMachineLevle attackPattern)
    {
        attackPattern.ActivatePatternEvent += OnActivatePattern;
    }

    public void Activate()
    {
        _isActive = true;
    }

    public void Deactivate()
    {
        _isActive = false;
    }

    private void OnActivatePattern(IStateMachinePattern newPattern)
    {
        if (_isActive == false)
            return;

        if (newPattern is GoToOpponentPattern)
            _effect.Play();
        else if (newPattern is StanPattern)
            _effect.Stop();
        else if (newPattern is WaitAfterAttackPattern)
            _effect.Stop();
        else if (newPattern is ShortAttackPattern)
            _effect.Stop();
    }
}
