using Cephei;
using System;
using UnityEngine;

public class PersonMover : MoverBase, IMover, IPersonComponent
{    
    [SerializeField, Range(0, 180)] private float _lockAngle = 90;
    [Space]
    [SerializeField] private MoverBase _mover;
    [SerializeField] private Rotator _rotator;

    public IMover Mover => _mover;
    public IMover Rotator => _rotator;

    public IPerson Person { get; private set; }

    public void Init(IPerson person)
    {
        Person = person;

        Mover.ComeToTarget += InvokeComeToTargetEvent;
    }

    private void Update()
    {
        if (IsMove == false) return;

        float angle = Vector3.Angle(Person.Forward, Person.Position.To(Target));

        if (_mover.IsMove && angle > _lockAngle)
            _mover.StopMove();

        if (_mover.IsMove == false && angle < _lockAngle)
            _mover.StartMove();
    }

    public override void SetTarget(Vector3 position)
    {
        base.SetTarget(position);

        _mover.SetTarget(position);
        _rotator.SetTarget(position);
    }

    public override void SetTarget(Transform targetTransform)
    {
        base.SetTarget(targetTransform);

        _mover.SetTarget(targetTransform);
        _rotator.SetTarget(targetTransform);
    }

    public override void StartMove()
    {
        base.StartMove();

        _mover.StartMove();
        _rotator.StartMove();

        Mover.ComeToTarget += InvokeComeToTargetEvent;
    }

    public override void StopMove()
    {
        base.StopMove();

        _mover.StopMove();
        _rotator.StopMove();

        Mover.ComeToTarget -= InvokeComeToTargetEvent;
    }
}