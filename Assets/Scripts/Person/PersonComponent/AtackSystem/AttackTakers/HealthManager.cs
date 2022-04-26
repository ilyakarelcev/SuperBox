using Cephei;
using System;
using UnityEngine;

public class HealthManager : MonoBehaviour, IAttackTaker
{
    [SerializeField] private float _startHealth = 20;

    public float Health { get; private set; }
    public float StartHealth => _startHealth;

    public event Action<float> ApplyDamageEvent;
    public event Action<float> AddHealEvent;
    public event Action<float> HealthChangedEvent;

    public event Action DieEvent;

    [Space]
    [SerializeField] private HealthManagerDebug _debug;
    public bool Log;

    [Header("Test")]
    public float TestDamage;
    public bool Test;

    private void Update()
    {
        if (Test)
        {
            Test = false;
            ApplyDamage(TestDamage);
        }
    }

    private void Start()
    {
        Health = _startHealth;
        _debug.HealthView = Health;
    }

    public void TakeAttack(Attack attack)
    {
        //if (attack.Damage <= 0) Debug.Log("Damage equals zero or less");

        ApplyDamage(attack.Damage);
    }

    public void AddHeal(float heal)
    {
        Health = Mathf.Min(Health + heal, _startHealth);

        _debug.HealthView = Health;

        AddHealEvent?.Invoke(heal);
        HealthChangedEvent?.Invoke(heal);
    }

    public void ApplyDamage(float damage)
    {
        if (BalanceSetup.PersonHitPerson)
            Health -= damage;

        _debug.HealthView = Health;

        ApplyDamageEvent?.Invoke(damage);
        HealthChangedEvent?.Invoke(damage);

        if (Health < 0) Die();
    }

    private void Die()
    {
        if (Log) Debug.Log(name + " die");
        //if (TryGetComponent(out Player player))
        //{
        //    Health = StartHealth;
        //    return;
        //}

        DieEvent?.Invoke();
        //Destroy(gameObject);
    }

    [System.Serializable]
    private class HealthManagerDebug
    {
        public float HealthView;
    }
}