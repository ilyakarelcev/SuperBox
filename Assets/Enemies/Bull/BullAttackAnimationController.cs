using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullAttackAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public event Action PreparationEndEvent;

    public bool IsPlayAttackAnimation { get; private set; }

    public void BeginPreparation()
    {
        if (IsPlayAttackAnimation) Debug.LogError("Call begin preparation when it playing");

        IsPlayAttackAnimation = true;
        _animator.SetTrigger("Preparation");
    }

    public void BreakAttack()
    {
        if (IsPlayAttackAnimation == false) return;

        IsPlayAttackAnimation = false;
        _animator.SetTrigger("BreakAttack");
    }

    public void OnPreparationEnd()
    {
        if (IsPlayAttackAnimation == false) return;

        PreparationEndEvent.Invoke();
    }

    public void EndAttack()
    {
        if (IsPlayAttackAnimation == false) return;

        IsPlayAttackAnimation = false;
        _animator.SetTrigger("EndAttack");
    }
}
