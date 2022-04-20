using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicViewAttacker : MonoBehaviour, IPersonComponent, IAttackView, IAttacker
{
    [SerializeField] private float _attackTime = 2;
    [Space]
    [SerializeField] private float _minRadius = 1;
    [SerializeField] private float _maxRadius = 4;
    [Space]
    [SerializeField] private AnimationCurve _radiusByTimeCurve;
    [SerializeField] private AnimationCurve _multiplyByRadiusCurve;
    [Space]
    [SerializeField] private ShortAttacker _shortAttacker;

    public event Action<Attack> FindPersonEvent;

    public event Action BeginingOfDamageEvent;
    public event Action EndingOfDamageEvent;
    public event Action StartAttackEvent;
    public event Action EndAttackEvent;

    public IPerson Person { get; private set; }

    public bool IsWork { get; private set; }

    private float _attackPercent;

    private CastomCoroutine _attackCoroutine;
    private CastomCoroutine _updateAttackPercentCoroutine;

    [Space]
    [Range(0, 1)] public float TestTime;
    public float RadiusView;
    public bool PlayAttack;
    [Space]
    public float DistanceToPerson;
    public Transform TestTransform;
    public float MultiplyView;

    private void Update()
    {
        float distanceToPerson = (TestTransform.position - Person.Position).magnitude;
        MultiplyView = _multiplyByRadiusCurve.Evaluate((distanceToPerson - _minRadius) / (_maxRadius - _minRadius));
        MultiplyView = Mathf.Max(MultiplyView, 0);

        return;
        if (PlayAttack)
        {
            if (_attackPercent == TestTime)
                _attackPercent = 0;

            UpdateAttackPercent();
        }
        else
            _attackPercent = TestTime;

        Attack();
    }

    public void Init(IPerson person)
    {
        Person = person;


        ///
        StartAttackEvent += StartAttack;
        EndAttackEvent += EndAttack;


        (this as IAttackView).StartAttack();
    }

    public void StartAttack()
    {
        _shortAttacker.FindPersonEvent += OnAttackPerson;
        _attackCoroutine = Person.Operator.OpenUpdateCoroutine(Attack, LifeType.Cycle);
    }

    public void EndAttack()
    {
        _shortAttacker.FindPersonEvent -= OnAttackPerson;

        _attackCoroutine.Destroy();
        _attackCoroutine = null;
    }

    private void Attack()
    {
        float t = _radiusByTimeCurve.Evaluate(_attackPercent);
        float radius = Mathf.Lerp(_minRadius, _maxRadius, t);
        _shortAttacker._radiusAttack = radius;

        _shortAttacker.Direction = Person.Forward;
        _shortAttacker.StartAttack();


        ///
        RadiusView = radius;

        float test = _multiplyByRadiusCurve.Evaluate(radius / _maxRadius);
        test = 0; // check on this place
    }

    private void OnAttackPerson(Attack attack)
    {
        float distanceToPerson = (attack.AttackedPerson.Position - Person.Position).magnitude;
        float multiply = _multiplyByRadiusCurve.Evaluate((distanceToPerson - _minRadius) / (_maxRadius - _minRadius));
        multiply = Mathf.Max(multiply, 0);

        attack.AttackMultiply = multiply;
        FindPersonEvent?.Invoke(attack);
    }

    void IAttackView.StartAttack()
    {
        IsWork = true;

        _updateAttackPercentCoroutine = Person.Operator.OpenUpdateCoroutine(UpdateAttackPercent, LifeType.Cycle);//_attackPercent = 0;
        StartAttackEvent?.Invoke();
        BeginingOfDamageEvent?.Invoke();
    }

    public void BreakAttack()
    {
        OnEndAttack();
    }

    private void UpdateAttackPercent()
    {
        _attackPercent += Time.deltaTime / _attackTime;


        return;////////
        if (_attackPercent >= 1)
            OnEndAttack();
    }

    private void OnEndAttack()
    {
        IsWork = false;
        _updateAttackPercentCoroutine.Destroy();
        _updateAttackPercentCoroutine = null;

        EndingOfDamageEvent?.Invoke();
        EndAttackEvent?.Invoke();
    }
}
