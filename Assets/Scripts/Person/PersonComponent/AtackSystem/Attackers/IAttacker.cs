using System;

public interface IAttacker
{
    event Action<IPerson, float> AttackEvent;

    void Attack();
}