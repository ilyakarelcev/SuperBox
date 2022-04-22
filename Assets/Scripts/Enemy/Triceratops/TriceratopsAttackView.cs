using System;
using UnityEngine;

public class TriceratopsAttackView : MonoBehaviour, IAttackView
{
    [SerializeField] private Animator _animator;

    public bool IsWork { get; private set; }
    public event Action BeginingOfDamageEvent;
    public event Action EndingOfDamageEvent;

    public event Action StartAttackEvent;
    public event Action EndAttackEvent;

    public event Action BeginJerkEvent;

    private bool _isEndingOfDamageToBe;

    [Space]
    public bool Log;

    public void StartAttack()
    {
        if (IsWork) return;

        _animator.SetTrigger("Attack");
        IsWork = true;
        _isEndingOfDamageToBe = false;

        StartAttackEvent?.Invoke();

        if (Log) Debug.Log("Play attack anim");
    }

    public void BreakAttack()
    {
        if (IsWork == false) return;

        _animator.SetTrigger("BreakAttack");
        IsWork = false;

        if (_isEndingOfDamageToBe == false)
            EndingOfDamageEvent.Invoke();
        EndAttackEvent?.Invoke();

        if (Log) Debug.Log("Break attack anim");
    }

    public void OnBegginingOfDamage()
    {
        if (IsWork == false) return; // This need where breake animation work but event work too;

        BeginingOfDamageEvent?.Invoke();

        if (Log) Debug.Log("On attack Event");
    }

    public void OnEndingOfDamage()
    {
        if (IsWork == false) return; // This need where breake animation work but event work too;

        _isEndingOfDamageToBe = true;
        EndingOfDamageEvent?.Invoke();

        if (Log) Debug.Log("On attack Event");
    }

    public void OnEndAnimation()
    {
        if (IsWork == false) return; // This need where breake animation work but event work too;

        IsWork = false;

        EndAttackEvent?.Invoke();

        if (Log) Debug.Log("On end attack event");
    }

    public void OnBeginigOfJerk()
    {
        if (IsWork == false) return;

        BeginJerkEvent?.Invoke();
    }
}