using Cephei;
using UnityEngine;

[System.Serializable]
public class BoxPushAway : IAttackHandler, IPersonComponent
{
    [SerializeField] private float pushAwayVelosity = 6;
    [Space]
    [SerializeField] private float _smoth = 1;

    public IPerson Person { get; private set; }

    private IJumpInfoConteiner _jumpInfo;

    [SerializeField] private BoxPushAwayDebug _debug;

    [Space]
    [Range(0, 1)]
    public float TestAngle;
    [Range(0, 1)]
    public float smothAngleView;
    [Space]
    public Transform TestTransform;

    public void Init(IPerson person)
    {
        Person = person;

        _jumpInfo = person.GetPersonComponentIs<IJumpInfoConteiner>();

        person.Operator.OpenUpdateCoroutine(CustomUpdate, LifeType.Cycle);
    }

    private void CustomUpdate()
    {
        return;

        TestAngle = Vector3.Angle(TestTransform.forward, TestTransform.forward.ZeroY());
        smothAngleView = SmothAngle(TestAngle);

        Debug.DrawRay(TestTransform.position, TestTransform.forward, Color.green);
        Debug.DrawRay(TestTransform.position, GetSmothVector(TestTransform.forward), Color.red);
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
        Vector3 smothVectorToEnemy = GetSmothVector(vectorFromEnemy);
        Vector3 velosity = smothVectorToEnemy.normalized * pushAwayVelosity * velosityPercent;

        Person.Rigidbody.AddForce(velosity, ForceMode.VelocityChange);

        _debug.SetVelosity(velosity, Person.Rigidbody.velocity, _jumpInfo.VelosityPercent);

        
        //Debug
        Debug.DrawRay(Person.Position, vectorFromEnemy, Color.yellow);
        Debug.DrawRay(Person.Position, velosity, Color.green);
        Debug.DrawRay(Person.Position, Person.Rigidbody.velocity + velosity, Color.red);
    }

    private Vector3 GetSmothVector(Vector3 toEnemy)
    {
        float angle = 90 - Vector3.Angle(toEnemy, Vector3.up);
        float smothAngle = SmothAngle(angle);

        _debug.SetAngles(angle, smothAngle);

        Vector3 toEnemyRight = toEnemy.GetRight();
        Vector3 onPlane = toEnemy.ZeroY();
        if (onPlane == Vector3.zero)
            onPlane = Random.onUnitSphere.ZeroY();

        return Quaternion.AngleAxis(smothAngle, toEnemyRight) * onPlane;
    }

    private float SmothAngle(float angle)
    { 
        float angleKoif = Mathf.InverseLerp(0 , 90, angle);
        float smothKoif = Mathf.Sqrt(angleKoif) * _smoth;
        float clampedSmothKoif = Mathf.Min(angleKoif, smothKoif);
        return clampedSmothKoif * 90;
    }

    [System.Serializable]
    private class BoxPushAwayDebug
    {
        public float Impuls;
        public float VelosityBefore;
        public float VelosityAfter;

        public float AngleBefore;
        public float AngleAfter;

        public float JumpMultiply;

        public void SetVelosity(Vector3 impuls, Vector3 velosityBefore, float jumpMultiply)
        {
            Impuls = impuls.magnitude;
            VelosityBefore = velosityBefore.magnitude;
            VelosityAfter = (impuls + velosityBefore).magnitude;

            JumpMultiply = jumpMultiply;
        }

        public void SetAngles(float angleBefore, float angleAfter)
        {
            AngleBefore = angleBefore;
            AngleAfter = angleAfter;
        }
    }
}
