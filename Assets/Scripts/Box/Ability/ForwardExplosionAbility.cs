using Cephei;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardExplosionAbility : MonoBehaviour, IPersonComponent
{
    [SerializeField] private float _pushAwayVelosityMultiply = 1.5f;
    [Space]
    [SerializeField] private ShortAttacker _attacker;
    [SerializeField] private DamageDealer _damageDealer;
    [SerializeField] private ImpulsDealer _impulsDealer;
    [Space]
    [SerializeField] private AbilityButton _button;
    [Space]
    [SerializeField] private ParticleSystem _effect;

    public IPerson Person { get; private set; }

    private IJumpInfoConteiner _jumpInfo;

    public void Init(IPerson person)
    {
        Person = person;
        _jumpInfo = person.GetPersonComponentIs<IJumpInfoConteiner>();

        _attacker.AttackEvent += OnAttack;
        _button.ClicEvent += Use;
    }

    public void Use()
    {
        if (_jumpInfo.IsJump)
        {
            _attacker.Attack(_jumpInfo.Direction);

            PushAway();
            PlayEffect();
        }
    }

    public void OnAttack(Attack attack)
    {
        attack.AttackingPerson = Person;

        _damageDealer.Handle(attack);
        _impulsDealer.Handle(attack);

        attack.AttackedPerson.AttackTakerManager.TakeAttack(attack);
    }

    public void PushAway()
    {
        Vector3 velosity = -1 * Person.Rigidbody.velocity * _pushAwayVelosityMultiply;
        Person.Rigidbody.AddForce(velosity, ForceMode.VelocityChange);
    }

    private void PlayEffect()
    {
        if (_keepCoroutine != null)
            StopCoroutine(_keepCoroutine);

        _keepCoroutine = StartCoroutine(KeepRoation(_jumpInfo.Direction));
        _effect.Play();
    }

    Coroutine _keepCoroutine;
    private IEnumerator KeepRoation(Vector3 direction)
    {
        while (true)
        {
            _effect.transform.rotation = direction.GetRotation();
            yield return null;
        }
    }
}
