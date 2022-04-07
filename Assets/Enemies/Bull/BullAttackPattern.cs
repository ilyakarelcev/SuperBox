using Cephei;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BullAttackPattern : MonoBehaviour
{
    [SerializeField] private float _distanceFromPlayerForAttack = 10;
    [SerializeField] private float _distanceOfAttack = 12;
    [SerializeField] private float _timeToAttack = 3;
    [SerializeField] private float _timeInStan = 0.4f;
    [Space]
    //[SerializeField] private Enemy _person;
    [SerializeField] private BullAttackAnimationController _animationController;
    [Space]
    [SerializeField] private SimpleMover _mover;
    [SerializeField] private SimpleMover _attackMover;
    [Space]
    [SerializeField] private TriggerAttacker _attacker;
    
    [Space]
    [SerializeField] private GameObject[] _attackHendlersGo;
    private IAttackHandler[] _attackHendlers;

    private bool _inStan;

    private float _timer;

    [Space]
    public Transform PlayerTransform;

    private void Start()
    {
        //_person.HealthManager.ApplyDamageEvent += OnApplyDamage;

        _animationController.PreparationEndEvent += OnPreparationEnd;

        _mover.SetTarget(PlayerTransform);

        _attackHendlers = _attackHendlersGo.Select(x => x.GetComponent<IAttackHandler>()).Where(x => x != null).ToArray();
        //foreach (var item in _attackHendlers)
        //{
        //    if (BalanceSetup.EnemyHitToEnemy)
        //        _attacker.AttackEvent += (p) => item.Handle(p, 1);
        //    else
        //        _attacker.AttackEvent += (p) => { if ((p is Enemy) == false) item.Handle(p, 1); };
        //}
        //_attacker.AttackEvent += (p) => EndAttack();

        _timer = _timeToAttack;
    }

    private void Update()
    {
        if (_inStan || _animationController.IsPlayAttackAnimation) return;

        _timer += Time.deltaTime;
        if (_timer < _timeToAttack) return;

        _mover.StartMove();

        Vector3 fromPlayer = transform.position - PlayerTransform.position;

        if (fromPlayer.sqrMagnitude < _distanceFromPlayerForAttack.Sqr())
        {
            _mover.StopMove();
            _animationController.BeginPreparation();

            _timer = 0;
        }
    }

    private void OnPreparationEnd()
    {
        //_mover.StopRotate();

        _attackMover.StartMove();
        _attackMover.ComeToTarget += EndAttack;

        Vector3 attackTarget = (transform.position + transform.forward * _distanceOfAttack).ZeroY();
        _attackMover.SetTarget(attackTarget);

        _attacker.Active();        
    }

    private void EndAttack()
    {
        _attackMover.ComeToTarget -= EndAttack;
        _attackMover.StopMove();
        //_attackMover.StopRotate();

        //_mover.StartRotate();

        _animationController.EndAttack();

        _attacker.Deactive();
    }

    private void OnApplyDamage()
    {
        _inStan = true;

        _animationController.BreakAttack();
        _attackMover.StopMove();
        //_attackMover.StopRotate();

        //_mover.StopRotate();

        _attacker.Deactive();

        StartCoroutine(Coroutines.WaitToAction(ExitInStan, _timeInStan));
    }

    private void ExitInStan()
    {
        _inStan = false;
        //_mover.StartRotate();
    }
}
