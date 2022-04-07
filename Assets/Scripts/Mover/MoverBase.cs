using System;
using UnityEngine;

public abstract class MoverBase : MonoBehaviour, IMover // Then do it not Monobeahaviour
{
    public Vector3 Target
    {
        get
        {
            if (_targetTransform)
                return _targetTransform.position;

            return _targetPosition;
        }
    }

    public event Action ComeToTarget;
    protected void InvokeComeToTargetEvent() => ComeToTarget?.Invoke();

    public bool IsMove { get; protected set; } = true;


    protected Vector3 _targetPosition;
    protected Transform _targetTransform;

    public virtual void SetTarget(Vector3 position)
    {
        _targetPosition = position;
        _targetTransform = null;
    }

    public virtual void SetTarget(Transform targetTransform)
    {
        _targetTransform = targetTransform;
    }

    public virtual void StartMove()
    {
        IsMove = true;
    }

    public virtual void StopMove()
    {
        IsMove = false;
    }
}