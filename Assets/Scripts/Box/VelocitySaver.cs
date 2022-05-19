using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocitySaver : IPersonComponent, IAttackHandler
{
    public IPerson Person { get; private set; }

    private IJumpInfoConteiner _jumpInfo;

    public void Init(IPerson person)
    {
        Person = person;

        _jumpInfo = person.GetPersonComponentIs<IJumpInfoConteiner>();
    }

    public void Handle(Attack attack)
    {
        float lastFrameVelosity = _jumpInfo.LastFrameVelosity;


        attack.AttackedPerson.HealthManager.DieEvent += SaveVelocity;
        attack.EndAttackEvent +=
            () => attack.AttackedPerson.HealthManager.DieEvent -= SaveVelocity;


        void SaveVelocity()
        {
            float test = lastFrameVelosity;
            float testNow = _jumpInfo.LastFrameVelosity;
            float nowVelocity = Person.Rigidbody.velocity.magnitude;
            Person.Rigidbody.velocity = Person.Rigidbody.velocity.normalized * _jumpInfo.LastFrameVelosity;
        }
    }
}
