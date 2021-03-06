using System;
using UnityEngine;

public class Attack
{
    public Attack(IPerson attackingPerson, IPerson attackedPerson, Vector3 attackDirection, float attackMultiply, 
        Vector3 contactPoint)
    {
        AttackingPerson = attackingPerson;
        AttackedPerson = attackedPerson;

        AttackDirection = attackDirection;
        ContactPoint = contactPoint;
        AttackMultiply = attackMultiply;
    }

    public Action EndAttackEvent;

    public float Damage;

    public Vector3 Impuls;
    public bool AddClearImpuls;
    public Vector3 ImpulsPositionOffset;

    public float AttackCoificent;

    public IPerson AttackingPerson;
    public IPerson AttackedPerson;

    public Vector3 AttackDirection;
    public Vector3 ContactPoint; 

    public float AttackMultiply;
}
