using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Cephei;

public class BoxAttacker : MonoBehaviour, IPersonComponent
{
    [SerializeField] private float _timeToForgetPerson = 0.2f;

    public IPerson Person { get; private set; }

    private LinkedList<Rigidbody> _collisionsPersons = new LinkedList<Rigidbody>();

    private IAttackHandler[] _attackHandlers;

    private IJumpInfoConteiner _jumpInfo;

    [Space]
    public bool Log;

    public void Init(IPerson person)
    {
        Person = person;
        _attackHandlers = person.GetAllPersonComponentIs<IAttackHandler>();
        _jumpInfo = person.GetPersonComponentIs<IJumpInfoConteiner>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_collisionsPersons.Contains(collision.rigidbody)) return;
        
        _collisionsPersons.AddLast(collision.rigidbody);
        StartCoroutine(ForgetPerson(collision.rigidbody));

        if (collision.rigidbody && collision.rigidbody.TryGetComponent(out IPerson person))
        {
            if (_jumpInfo.IsJump == false) return;

            HandleAttack(person, collision.contacts[0].point); // Important use collision.MidleContactPoint()

            if(Log) Debug.Log("Attack " + person.Rigidbody.name);
        }
    }

    private void HandleAttack(IPerson person, Vector3 contactPoint)
    {
        float multiply = _jumpInfo.VelosityPercent; 
        Vector3 direction = _jumpInfo.Direction;

        Attack attack = new Attack(Person, person, direction, multiply, contactPoint);

        CustomDebug.DrawCross(contactPoint, 1, Color.red);

        foreach (var handler in _attackHandlers)
        {
            handler.Handle(attack);
        }

        person.AttackTakerManager.TakeAttack(attack);
    }

    IEnumerator ForgetPerson(Rigidbody rigidbody)
    {
        yield return new WaitForSeconds(_timeToForgetPerson);
        _collisionsPersons.Remove(rigidbody);
    }
}
