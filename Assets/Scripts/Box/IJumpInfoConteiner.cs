using System;
using UnityEngine;

public interface IJumpInfoConteiner
{
    float MaxVelosity { get; }
    float Duration { get; }

    float VelosityPercent { get; }
    bool IsJump { get; }
    float LastFrameVelosity { get; }

    Vector3 Direction { get; }
    float Multiply { get; }
    float Charge { get; }
    float Input { get; }

    event Action<IJumpInfoConteiner> JumpEvent;
}

public interface IJumpInfoTaker
{
    public void TakeNewJumpInfo(Vector3 direction, float multiply, float charge, float input);
}

[System.Serializable]
public class JumpInfoConteiner : IJumpInfoConteiner, IJumpInfoTaker, IPersonComponent
{
    [SerializeField] private float _maxVelosity = 22;
    [SerializeField] private float _duration = 1;
    [SerializeField] private Rigidbody _rigidbody;

    public float MaxVelosity => _maxVelosity;

    public float Duration => _duration;


    public float VelosityPercent => _lastFrameVelosity / MaxVelosity;

    public bool IsJump => _lastFrameVelosity > 1;// 1 is jump drop

    public float LastFrameVelosity => _lastFrameVelosity;


    public Vector3 Direction { get; private set; }

    public float Multiply { get; private set; }

    public float Charge { get; private set; }

    public float Input { get; private set; }


    public event Action<IJumpInfoConteiner> JumpEvent;


    public IPerson Person { get; private set; }

    private float _lastFrameVelosity;    

    public void Init(IPerson person)
    {
        Person = person;

        person.Operator.OpenUpdateCoroutine(
            () => _lastFrameVelosity = _rigidbody.velocity.magnitude, 
            LifeType.Cycle, UpdateType.FixedUpdate);
    }

    public void TakeNewJumpInfo(Vector3 direction, float multiply, float charge, float input)
    {
        Direction = direction;
        Multiply = multiply;
        Charge = charge;
        Input = input;

        JumpEvent?.Invoke(this);
    }
}
