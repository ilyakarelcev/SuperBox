using Cephei;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShootingAttackPattern : MonoBehaviour
{
    [SerializeField] private float _distanceToSeePlayer = 20;
    [SerializeField] private float _shootPeriod = 3;
    [Space]
    [SerializeField] private float _timeInStan = 0.4f;
    [Space]
    [SerializeField] private AttackAnimationView _animationControler;
    //[SerializeField] private Enemy _selfPerson; // then swich Enemy on Person and take it from hight
    [SerializeField] private MoverBase _mover;
    [SerializeField] private Shooter _shooter;
    [Space]
    [SerializeField] private GameObject[] _attackHendlersGo;
    private IAttackHandler[] _attackHendlers;

    public Transform _playerTransform;

    private bool _inStan;

    private float _timer;

    private void Start()
    {
        _animationControler.BeginingOfDamageEvent += _shooter.Attack;
        _animationControler.EndAttackEvent += OnEndAttack;

        //_selfPerson.HealthManager.ApplyDamageEvent += OnApplyDamage;

        _attackHendlers = _attackHendlersGo.Select(x => x.GetComponent<IAttackHandler>()).Where(x => x != null).ToArray();
        foreach (var item in _attackHendlers)
        {
            //if (BalanceSetup.EnemyHitToEnemy)
            //    _shooter.AttackEvent += (p) =>
            //    {
            //        if (p == _selfPerson) return;
            //        item.Handle(p, 1);
            //    };
            //else
            //    _shooter.AttackEvent += (p) => { if ((p is Enemy) == false) item.Handle(p, 1); };
            
        }

        _timer = _shootPeriod;
    }

    private void OnApplyDamage()
    {
        _inStan = true;
        _animationControler.BreakAttack();
        _mover.StopMove();

        StartCoroutine(Coroutines.WaitToAction(ExitInStan, _timeInStan));
    }

    private void Update()
    {
        if (_inStan || _animationControler.IsWork  
            || Vector3.Distance(transform.position, _playerTransform.position) > _distanceToSeePlayer)
            return; 

        if (_timer > _shootPeriod)
        {
            _timer -= _shootPeriod;

            _animationControler.StartAttack();
            _shooter.SetTarget(_playerTransform);
        }

        _mover.SetTarget(_playerTransform);
        _timer += Time.deltaTime;
    }

    private void OnEndAttack()
    {
        if (_inStan) Debug.LogError("Error. AttackAnimation controller work not correctly");

        _mover.StartMove();
    }

    private void ExitInStan()
    {
        _inStan = false;
        _mover.StartMove();
        _timer = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _distanceToSeePlayer);
    }
}
