using Cephei;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private float _bulletSpeed = 5;
    [SerializeField] private Bullet _buletPrefab;
    [Space]
    [SerializeField] private Transform _shootPoint;

    public bool IsAttack { get; private set; }

    public event Action<Attack> AttackEvent;

    private Transform _target;

    public void Attack()
    {
        ShootToForward();
    }

    private void AttackToTarget()
    {
        Vector3 toTarget = _target.position.ZeroY() - transform.position.ZeroY();

        Bullet bullet = Instantiate(_buletPrefab, _shootPoint.position, toTarget.GetRotation());
        bullet.Init(toTarget.normalized, _bulletSpeed);

        //bullet.OnPersonCollision += (p) => AttackEvent.Invoke(p, 1);
    }

    private void ShootToForward()
    {
        Bullet bullet = Instantiate(_buletPrefab, _shootPoint.position, _shootPoint.forward.GetRotation());
        bullet.Init(_shootPoint.forward, _bulletSpeed);

        bullet.OnPersonCollision += OnPersonCollision;
    }

    private void OnPersonCollision(Attack attack)
    {
        AttackEvent.Invoke(attack);
    }

    public void SetTarget(Transform target) => _target = target; 
}