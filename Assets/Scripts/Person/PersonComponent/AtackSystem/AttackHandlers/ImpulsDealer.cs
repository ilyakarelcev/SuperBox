using Cephei;
using UnityEngine;

public class ImpulsDealer : MonoBehaviour, IAttackHandler
{
    [SerializeField] private float _force = 5;
    [SerializeField, Range(0, 1)] private float _upPercent = 0.5f;
    [SerializeField, Range(0, 1)] private float _sidePercent = 0.2f;
    [SerializeField] private float _positionOffset = 1;
    [Space]
    [SerializeField] private bool _zeroY;

    [Space]
    [SerializeField] private ImpulsDealerDebug _debug;

    public void Handle(Attack attack)
    {
        attack.Impuls = CalculateImpuls(attack.AttackDirection, _force * attack.AttackMultiply, _upPercent, _sidePercent);
        _debug.TakeImpuls(attack.Impuls);
    }

    private void OldVersion(Attack attack)
    {
        Vector3 toPerson = attack.AttackedPerson.Position - attack.AttackingPerson.Position;
        if (_zeroY)
            toPerson = toPerson.ZeroY();

        attack.Impuls = CalculateImpuls(toPerson.normalized, _force * attack.AttackMultiply, _upPercent, _sidePercent);
        attack.ImpulsPositionOffset = -toPerson.normalized * _positionOffset;
    }

    private Vector3 CalculateImpuls(Vector3 direction, float force, float upPercent, float sidePercent)
    {
        Vector3 forwardForce = direction * force * (1 - upPercent - sidePercent);
        Vector3 upForce = Vector3.up * force * upPercent;
        Vector3 sideForce = Vector3.Cross(direction, Vector3.up) * force * new System.Random().Sign() * _sidePercent;

        return forwardForce + upForce + sideForce;
    }

    [System.Serializable]
    private class ImpulsDealerDebug
    {
        public Vector3 Impuls;
        public float ImpulsMagnitude;

        public void TakeImpuls(Vector3 impuls)
        {
            Impuls = impuls;
            ImpulsMagnitude = impuls.magnitude;
        }
    }
}