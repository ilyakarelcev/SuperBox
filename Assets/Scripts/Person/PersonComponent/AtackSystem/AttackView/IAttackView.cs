using System;

public interface IAttackView
{
    public bool IsWork { get; }

    public event Action BeginingOfDamageEvent;
    public event Action EndingOfDamageEvent;

    public event Action StartAttackEvent;
    public event Action EndAttackEvent;

    public void StartAttack();

    public void BreakAttack();
}
