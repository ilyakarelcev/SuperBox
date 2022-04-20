﻿using Cephei;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleImpulsAbility : AbilityBase, IPersonComponent
{
    [SerializeField] private float _radius = 6;
    [SerializeField] private AnimationCurve _impulsByDistance;
    [Space]
    [SerializeField] private ImpulsDealer _impulsDealer;
    [SerializeField] private AbilityButton _button;
    [Space]
    [SerializeField] private ParticleSystem _effect;

    public IPerson Person { get; private set; }

    public void Init(IPerson person)
    {
        Person = person;

        Init();
    }

    public override void Use()
    {
        base.Use();

        HashSet<IPerson> persons = new HashSet<IPerson>();
        foreach (var collider in Physics.OverlapSphere(Person.Position, _radius))
        {
            if (collider.isTrigger) continue;

            IPerson person = IPerson.GetPersonFromRigidbody(collider.attachedRigidbody);

            if(person != null && person != Person)
            {
                persons.Add(person);
            }
        }
        foreach (var person in persons)
        {
            AttackPerson(person);
        }

        PlayEffects();        
    }

    private void AttackPerson(IPerson person)
    {
        Vector3 attackDirection = person.Position.From(Person.Position);
        float multiply = attackDirection.sqrMagnitude / _radius.Sqr();
        multiply = _impulsByDistance.Evaluate(multiply);

        Attack attack = new Attack(Person, person, attackDirection.normalized, multiply, person.Position);

        _impulsDealer.Handle(attack);       

        person.AttackTakerManager.TakeAttack(attack);
    }

    private void PlayEffects()
    {
        _effect.Play();
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying == false) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Person.Position, _radius);
    }
}