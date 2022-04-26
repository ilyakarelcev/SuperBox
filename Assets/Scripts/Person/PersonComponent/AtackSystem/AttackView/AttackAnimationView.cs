using System;
using UnityEngine;

// Не очень понятно нужно ли вызывать эвенты об окончании атаки и конце демеджа из метода BreakAttack. Оставлю ответ на этот вопрос на будущее

public class AttackAnimationView : MonoBehaviour, IAttackView
{
    [SerializeField] private Animator _animator;

    public bool IsWork { get; private set; }
    public event Action BeginingOfDamageEvent;
    public event Action EndingOfDamageEvent;

    public event Action StartAttackEvent;
    public event Action EndAttackEvent;

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
            EndingOfDamageEvent?.Invoke();
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
}