using Cephei;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShortAttackPatternOld : MonoBehaviour
{
    [SerializeField] private float _distanceToAttack = 0.7f;
    [SerializeField] private float _distanceToPlayer = 0.5f;
    [SerializeField] private float _timeInStan = 0.4f;
    [Space]
    [SerializeField] private SimpleMover _mover;
    [SerializeField] private ShortAttacker _attacker;
    [SerializeField] private AttackAnimationView _animationControler;
    //[SerializeField] private Enemy _enemy;
    //[Space]
    [SerializeField] private GameObject[] _attackHendlersGo;
    private IAttackHandler[] _attackHendlers;

    public Transform _playerTransform;

    private bool _inStan;

    private void Start()
    {
        _mover.SetTarget(_playerTransform);
        _mover.StartMove();
        //_enemy.HealthManager.ApplyDamageEvent += OnApplyDamage;


        //_animationControler.BeginingOfDamageEvent += _attacker.Attack;
        _animationControler.EndAttackEvent += OnAttackEnd;


        _attackHendlers = _attackHendlersGo.Select(x => x.GetComponent<IAttackHandler>()).Where(x => x != null).ToArray();
        foreach (var item in _attackHendlers)
        {
            //if(BalanceSetup.EnemyHitToEnemy)
            //    _attacker.AttackEvent += (p) => item.Handle(p, 1);
            //else
            //    _attacker.AttackEvent += (p) => { if ((p is Enemy) == false) item.Handle(p, 1); };
        }
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
        if (_inStan || _animationControler.IsWork) return;

        Vector3 fromPlayer = transform.position - _playerTransform.position;
        _mover.SetTarget(_playerTransform.position + fromPlayer.normalized * _distanceToPlayer);

        if(fromPlayer.sqrMagnitude < _distanceToAttack.Sqr())
        {
            _mover.StopMove();
            _animationControler.StartAttack();
        }
    }

    private void OnAttackEnd()
    {
        if (_inStan) Debug.LogError("Error. AttackAnimation controller work not correctly");

        _mover.StartMove();
    }

    private void ExitInStan()
    {
        _inStan = false;
        _mover.StartMove();
    }
}