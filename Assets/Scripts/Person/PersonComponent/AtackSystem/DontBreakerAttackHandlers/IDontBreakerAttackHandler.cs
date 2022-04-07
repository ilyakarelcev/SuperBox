using System;
using System.Collections;

public interface IDontBreakerAttackHandler
{
    public event Action BreakEvent;

    public void StartBreaker();

    public void EndBreaker();
}
