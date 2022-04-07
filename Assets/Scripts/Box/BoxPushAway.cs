using UnityEngine;

[System.Serializable]
public class BoxPushAway : IAttackHandler, IPersonComponent
{
    [SerializeField] private float pushAwayVelosity = 6;

    public IPerson Person { get; private set; }

    private IJumpInfoConteiner _jumpInfo;

    [SerializeField] private BoxPushAwayDebug _debug;

    public void Init(IPerson person)
    {
        Person = person;

        _jumpInfo = person.GetPersonComponentIs<IJumpInfoConteiner>();
    }

    public void Handle(Attack attack)
    {
        HealthManager healthManager = attack.AttackedPerson.HealthManager;
        float velosityPercent = _jumpInfo.VelosityPercent;

        Person.Operator.OpenUpdateCoroutine(CheckOnDie, LifeType.OneShot, UpdateType.FixedUpdate);

        void CheckOnDie()
        {
            if (healthManager.Health > 0)
                PushAway(attack, velosityPercent);
        }        
    }

    private void PushAway(Attack attack, float velosityPercent)
    {
        Debug.DrawRay(Person.Position, Person.Rigidbody.velocity, Color.blue);

        Vector3 vectorFromEnemy = Person.Position - attack.AttackedPerson.Position;
        Vector3 velosity = vectorFromEnemy.normalized * pushAwayVelosity * velosityPercent;

        Person.Rigidbody.AddForce(velosity, ForceMode.VelocityChange);

        _debug = new BoxPushAwayDebug(velosity, Person.Rigidbody.velocity, _jumpInfo.VelosityPercent);


        Debug.DrawRay(Person.Position, velosity, Color.green);
        Debug.DrawRay(Person.Position, Person.Rigidbody.velocity + velosity, Color.red);

        //CepheiExtantion.CustomDebug.Break();
    }

    [System.Serializable]
    private class BoxPushAwayDebug
    {
        public float Impuls;
        public float VelosityBefore;
        public float VelosityAfter;

        public float JumpMultiply;

        public BoxPushAwayDebug(Vector3 impuls, Vector3 velosityBefore, float jumpMultiply)
        {
            Impuls = impuls.magnitude;
            VelosityBefore = velosityBefore.magnitude;
            VelosityAfter = (impuls + velosityBefore).magnitude;
            JumpMultiply = jumpMultiply;
        }
    }
}
