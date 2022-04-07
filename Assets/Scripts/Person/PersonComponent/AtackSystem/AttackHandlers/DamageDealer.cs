using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour, IAttackHandler
{
    [SerializeField] private float _damage = 2;

    public void Handle(Attack attack)
    {
        attack.Damage += _damage * attack.AttackMultiply;
    }
}