using System;
using UnityEngine;

public interface IAttacker
{
    Vector3 Direction { get; set; }

    event Action<Attack> FindPersonEvent;

    void StartAttack();
    void EndAttack();
}