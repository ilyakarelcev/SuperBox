using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxAttackHandler : MonoBehaviour, IAttackHandler, IPersonComponent
{
    [SerializeField] private float _damage = 10;
    [SerializeField] private float AddedImpuls = 5;
    [Space]
    [SerializeField] private AnimationCurve _damageOnJumpDuration;

    [Space]
    [SerializeField] private BoxAttackerDebug _debug;

    public IPerson Person { get; private set; }

    private IJumpInfoConteiner _jumpInfo;

    [Space]
    public bool Log;

    public void Init(IPerson person)
    {
        Person = person;

        _jumpInfo = person.GetPersonComponentIs<IJumpInfoConteiner>();
    }

    public void Handle(Attack attack)
    {
        float damage = _damage * _damageOnJumpDuration.Evaluate(_jumpInfo.VelosityPercent);
        attack.Damage = damage;

        float force = AddedImpuls * _jumpInfo.VelosityPercent;
        attack.Impuls = _jumpInfo.Direction * force;

        _debug.TakeHitInfo(damage, attack.AttackedPerson.Rigidbody.name, force, _jumpInfo.VelosityPercent, 0);
    }

    [System.Serializable]
    private struct BoxAttackerDebug
    {
        public float HitDamage;
        public float HitForce;
        public string EnemyName;

        public float JumpMultiply;
        public float TimePercent;

        public void TakeHitInfo(float damage, string enemyName, float hitForce, float jumpMultiply, float timePercent)
        {
            HitDamage = damage;
            HitForce = hitForce;
            EnemyName = enemyName;

            JumpMultiply = jumpMultiply;
            TimePercent = timePercent;
        }
    }
}