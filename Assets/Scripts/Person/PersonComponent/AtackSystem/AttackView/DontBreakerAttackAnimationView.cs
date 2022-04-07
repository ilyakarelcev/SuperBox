using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontBreakerAttackAnimationView : MonoBehaviour, IDontBreakerAttackView
{
    [SerializeField] private Animator _animator;

    public bool IsWork { get; private set; }

    public bool IsDontBreakState { get; private set; }

    public event Action BeginingOfDamageEvent;
    public event Action EndingOfDamageEvent;

    public event Action EndAttackEvent;
    public event Action StartAttackEvent;

    public event Action BeginingDontBreakStateEvent;
    public event Action EndingDontBreakStateEvent;

    [Space]
    public bool Log;

    public void StartAttack()
    {
        if (IsWork) return;

        _animator.SetTrigger("Attack");
        IsWork = true;

        StartAttackEvent?.Invoke();

        if (Log) Debug.Log("Play attack anim");
    }

    public void BreakAttack()
    {
        if (IsWork == false) return;

        _animator.SetTrigger("BreakAttack");
        IsWork = false;

        EndAttackEvent?.Invoke();

        if (Log) Debug.Log("Break attack anim");
    }

    public void OnBeginingOfDamage()
    {
        if (IsWork == false) return; // This need where breake animation work but event work too;

        BeginingOfDamageEvent?.Invoke();

        if (Log) Debug.Log("On attack Event");
    }

    public void OnEndingOfDamage()
    {
        if (IsWork == false) return; // This need where breake animation work but event work too;

        EndingOfDamageEvent?.Invoke();

        if (Log) Debug.Log("On attack Event");
    }

    public void OnEndAnimation()
    {
        if (IsWork == false) return; // This need where breake animation work but event work too;
        IsWork = false;

        if (IsDontBreakState) OnEndingDontBreakState();

        EndAttackEvent?.Invoke();

        if (Log) Debug.Log("On end attack event");
    }

    public void OnBeginingDontBreakState()
    {
        if (IsWork == false) return;

        IsDontBreakState = true;
        BeginingDontBreakStateEvent?.Invoke();

        if (Log) Debug.Log("On BeginingDontBreakState");
    }

    public void OnEndingDontBreakState()
    {
        if (IsWork == false) return;

        IsDontBreakState = false;
        EndingDontBreakStateEvent?.Invoke();

        if (Log) Debug.Log("On BeginingDontBreakState");
    }
}