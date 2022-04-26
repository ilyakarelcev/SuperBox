using Cephei;
using System;
using UnityEngine;

public class EnemyVision : MonoBehaviour, IPersonComponent
{
    public bool PlayerIsVision { get; private set; }
    public bool PlayerIsInside { get; private set; }

    public IPerson Person { get; private set; }

    public event Action<bool> ChangeIsPlayerVisionEvent;

    private Transform _player;
    private CastomCoroutine _rayCastInPlayer;

    public void Init(IPerson person)
    {
        Person = person;        
    }

    public void SetRadius(float radius)
    {
        transform.localScale = Vector3.one * radius;
    }

    private void LateUpdate()
    {
        transform.position = Person.Position.ZeroY() + Vector3.up * 0.01f;
        transform.rotation = Person.Forward.ZeroY().GetRotation();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (PlayerIsVision || PlayerIsInside) return;

        IPerson person = IPerson.GetPersonFromRigidbody(other.attachedRigidbody);
        if (person is Player)
        {
            _rayCastInPlayer = Person.Operator.OpenUpdateCoroutine(RayCastInPlayer, LifeType.Cycle);

            _player = person.Transform;
            PlayerIsInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (PlayerIsInside == false) return;

        if (IPerson.GetPersonFromRigidbody(other.attachedRigidbody) is Player)
        {
            PlayerIsInside = false;
            _rayCastInPlayer?.Destroy();

            if (PlayerIsVision)
            {
                PlayerIsVision = false;
                ChangeIsPlayerVisionEvent?.Invoke(false);
                return;
            }
        }
    }

    private void RayCastInPlayer()
    {
        Physics.Raycast(Person.Position, Person.Position.To(_player.position), out RaycastHit hit, float.PositiveInfinity, -1, QueryTriggerInteraction.Ignore);

        if (IPerson.GetPersonFromRigidbody(hit.collider.attachedRigidbody) is Player)
        {
            PlayerIsVision = true;
            ChangeIsPlayerVisionEvent?.Invoke(true);

            _rayCastInPlayer.Destroy();
        }
    }    
}
