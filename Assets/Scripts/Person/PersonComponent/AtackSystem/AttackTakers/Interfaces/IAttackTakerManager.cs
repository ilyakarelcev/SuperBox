using System;
using System.Collections.Generic;

public interface IAttackTakerManager
{
    public List<Func<Attack, bool>> Conditions { get; set; }
    public List<IAttackTaker> AttackTakers { get; set; }

    public event Action<Attack> ConditionsForEachEndEvent;
    public event Action<Attack> AttackTakersForEachEndEvent;

    public event Action<Attack> OnTakeAttack;

    public Attack CurentAttack { get; }

    public bool TakeAttack(Attack attack);
}
