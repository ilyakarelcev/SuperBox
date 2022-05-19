using Cephei;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TriceratopsAttackInterpretator : AttackInterpretator, IAttackTaker
{
    [SerializeField] private float _impulsMultiplyOnCircleAttack = 0.7f;

    public new void TakeAttack(Attack attack)
    {
        base.TakeAttack(attack);

        if (attack is CircleImpulsAttack)
            attack.Impuls = attack.Impuls.ZeroY() * _impulsMultiplyOnCircleAttack;
    }
}
