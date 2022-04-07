using System;
using UnityEngine;

public interface IMover
{
    Vector3 Target { get; }

    bool IsMove { get; }

    event Action ComeToTarget;

    void SetTarget(Vector3 position);

    void SetTarget(Transform targetTransform);

    void StartMove();

    void StopMove();
}