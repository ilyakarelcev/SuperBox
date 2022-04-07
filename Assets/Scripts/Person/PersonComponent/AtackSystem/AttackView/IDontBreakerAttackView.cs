using System;

public interface IDontBreakerAttackView : IAttackView
{
    public bool IsDontBreakState { get; }

    public event Action BeginingDontBreakStateEvent;
    public event Action EndingDontBreakStateEvent;
}
