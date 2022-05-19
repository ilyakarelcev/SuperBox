using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicViewAttacker : MonoBehaviour, IPersonComponent, IAttackView, IAttacker
{
    [SerializeField] private float _twoWaveTime = 1;
    [SerializeField] private float _attackTime = 2;
    [Space]
    [SerializeField] private float _minRadius = 1;
    [SerializeField] private float _maxRadius = 4;
    [Space]
    [SerializeField] private AnimationCurve _radiusByTimeCurve;
    [SerializeField] private AnimationCurve _multiplyByRadiusCurve;
    [Space]
    [SerializeField] private ShortAttacker _shortAttacker;
    [Space]
    [SerializeField] private MagicAttackZoneView _attackZoneView;

    public event Action<Attack> FindPersonEvent;
    public Vector3 Direction { get; set; }

    public event Action BeginingOfDamageEvent;
    public event Action EndingOfDamageEvent;
    public event Action StartAttackEvent;
    public event Action EndAttackEvent;

    public IPerson Person { get; private set; }

    public bool IsWork { get; private set; }    

    private float _attackPercent;

    private CastomCoroutine _attackCoroutine;
    private CastomCoroutine _updateAttackPercentCoroutine;

    private bool _isSecondWave;

    private LinkedList<IPerson> _attackedPerson = new LinkedList<IPerson>();

    [Space]
    [Range(0, 1)] public float TestTime;
    public float RadiusView;
    public bool PlayAttack;
    [Space]
    public float DistanceToPerson;
    public Transform TestTransform;
    public float MultiplyView;
    [Space]
    public bool Log;

    //Tools for setup values(curve, time and radiuse)
    private void Update()
    {
        return;
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
    }

    public void StartAttack()
    {
        _shortAttacker.FindPersonEvent += OnAttackPerson;
        _attackCoroutine = Person.Operator.OpenUpdateCoroutine(Attack, LifeType.Cycle);

        _attackZoneView.Show();
        _isSecondWave = false;
    }

    public void EndAttack()
    {
        _shortAttacker.FindPersonEvent -= OnAttackPerson;

        _attackCoroutine.Destroy();
        _attackCoroutine = null;

        _attackedPerson.Clear();

        _attackZoneView.Hide();
    }

    private void Attack()
    {
        if (_isSecondWave == false && _attackPercent > _twoWaveTime / _attackTime)
        {
            _attackedPerson.Clear();
            _isSecondWave = true;
        }

        float t = _radiusByTimeCurve.Evaluate(_attackPercent);
        float radius = Mathf.Lerp(_minRadius, _maxRadius, t);
        _shortAttacker._radiusAttack = radius;

        _shortAttacker.Direction = Direction;
        _shortAttacker.StartAttack();

        _attackZoneView.SetScale(radius);

        Debug.Log("Radius: " + radius);
    }

    private void OnAttackPerson(Attack attack)
    {
        if (_attackedPerson.Contains(attack.AttackedPerson))
            return;
        _attackedPerson.AddLast(attack.AttackedPerson);


        float distanceToPerson = (attack.AttackedPerson.Position - Person.Position).magnitude;
        float multiply = _multiplyByRadiusCurve.Evaluate((distanceToPerson - _minRadius) / (_maxRadius - _minRadius));
        multiply = Mathf.Max(multiply, 0);


        attack.AttackMultiply = multiply;
        attack.AddClearImpuls = true;
        FindPersonEvent?.Invoke(attack);
    }


    //AttackView//


    void IAttackView.StartAttack()
    {
        IsWork = true;
        _attackPercent = 0;

        _updateAttackPercentCoroutine = Person.Operator.OpenUpdateCoroutine(UpdateAttackPercent, LifeType.Cycle);
        StartAttackEvent?.Invoke();
        BeginingOfDamageEvent?.Invoke();
    }

    public void BreakAttack()
    {
        if(IsWork)
            OnEndAttack();
    }

    private void UpdateAttackPercent()
    {
        _attackPercent += Time.deltaTime / _attackTime;

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
