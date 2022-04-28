using System;
using System.Collections.Generic;

public class AttackTakerManagerBase : IAttackTakerManager
{
    public AttackTakerManagerBase(IAttackTaker attackIntarpretator)
    {
        AttackInterpretator = attackIntarpretator;
    }

    public List<Func<Attack, bool>> Conditions { get; set; } = new List<Func<Attack, bool>>();

    public List<IAttackTaker> AttackTakers { get; set; } = new List<IAttackTaker>();

    public event Action<Attack> ConditionsForEachEndEvent;
    public event Action<Attack> AttackTakersForEachEndEvent;
    public event Action<Attack> OnTakeAttack;

    public Attack CurentAttack { get; private set; }

    private IAttackTaker AttackInterpretator; 

    public virtual bool TakeAttack(Attack attack)
    {
        AttackInterpretator.TakeAttack(attack);

        CurentAttack = attack;
        OnTakeAttack?.Invoke(attack);

        foreach (var condition in Conditions)
        {
            if(! condition.Invoke(attack)) return false;
        }
        ConditionsForEachEndEvent?.Invoke(attack);


        foreach (var taker in AttackTakers)
        {
            taker.TakeAttack(attack);
        }
        AttackTakersForEachEndEvent?.Invoke(attack);

        return true;
    }
}