using Cephei;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShortAttacker : MonoBehaviour
{
    [SerializeField] private float _radiusAttack = 1;
    [SerializeField, Range(0, 360)] private float _angleAttack = 90;
    [Space]
    [SerializeField] private Rigidbody _selfRb;

    public bool IsAttack { get; private set; }

    public event Action<Attack> AttackEvent;

    public void Attack(Vector3 attackDirection)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radiusAttack);
        LinkedList<Rigidbody> _rigidbodyOnCurentHit = new LinkedList<Rigidbody>();

        foreach (var collider in colliders)
        {
            if (collider.isTrigger) continue;
            if (collider.attachedRigidbody == false) continue; 
            if (collider.attachedRigidbody == _selfRb) continue;
            if (_rigidbodyOnCurentHit.Contains(collider.attachedRigidbody)) continue;

            _rigidbodyOnCurentHit.AddLast(collider.attachedRigidbody);

            if (collider.attachedRigidbody.TryGetComponent(out IPerson person))
            {
                Vector3 toPerson = person.Transform.position - transform.position;

                if (Vector3.Angle(toPerson.ZeroY(), attackDirection.ZeroY()) < _angleAttack / 2)
                {
                    CreateAttackObject(person, toPerson);
                }
            }
        }
        _rigidbodyOnCurentHit.Clear();
    }

    private void CreateAttackObject(IPerson other, Vector3 toOther)
    {
        Vector3 direction = toOther.normalized;//GetDirectionToOtherPerson(other);
        Vector3 contactPoint = GetContactPointByDirection(direction);

        Attack attack = new Attack(null, other, direction, 1, contactPoint);
        AttackEvent?.Invoke(attack);
    }

    private Vector3 GetDirectionToOtherPerson(IPerson other)
    {
        return Vector3.zero;
        //Vector3 direction = other.Position - _selfPerson.Position;

        //Vector3 directionsRight = Vector3.Cross(direction, Vector3.up);

        //Vector3 directionOnRight = direction.ProjectOnPlane(_selfPerson.Transform.right);
        //if (Vector3.Dot(directionOnRight, _selfPerson.Forward) < 0)
        //    directionOnRight = Vector3.Reflect(direction, _selfPerson.Forward);


        //if (Vector3.Angle(directionOnRight, direction) > _angleAttack)
        //{
        //    float sign = Vector3.SignedAngle(direction.ZeroY(), directionOnRight.ZeroY(), Vector3.up).Sign();

        //    Vector3 directionsUp = Vector3.Cross(direction, directionsRight);
        //    Vector3 limitDirection = Quaternion.AngleAxis(_angleAttack, -1 * directionsUp) * directionOnRight;
        //    //Возможно достаточно просто умножить directionsUp на -1 и все заработает, и никакого высчитывания знака не нужно

        //    direction = limitDirection;

        //    Debug.DrawRay(transform.position, limitDirection, Color.red);
        //    Debug.DrawRay(transform.position, directionsUp, Color.green);
        //}

        //return direction;
    }

    private Vector3 GetContactPointByDirection(Vector3 direction)
    {
        Physics.Raycast(transform.position, direction, out RaycastHit hit);

        if (hit.distance > _radiusAttack * 2)
            Debug.LogError("Not correctly resault, point too far");

        return hit.point;
    }

    // План:
    // Посчитать вектор к другому персонажу
    // Померить для него угол, и если он меньше то взять лимитное направление
    // Стрельнуть по получившемуся направлению лучом
    // Записать эту полученную точку в точку контакта
    // Ну и записать направление

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(_angleAttack / 2, Vector3.up) * transform.forward * _radiusAttack, Color.blue + Color.red * 0.5f);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(-1 * _angleAttack / 2, Vector3.up) * transform.forward * _radiusAttack, Color.blue + Color.red * 0.5f);
    }
}
