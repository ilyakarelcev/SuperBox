using Cephei;
using UnityEngine;

[System.Serializable]
public class CollisionEfect : IAttackTaker, IPersonComponent
{
    [SerializeField] private ParticleSystem[] _effects;
    
    public IPerson Person { get; private set; }

    [Space]
    [SerializeField] private CollisionEffectDebug _debug;

    public void Init(IPerson person)
    {
        Person = person;
    }

    public void TakeAttack(Attack attack)
    {
        foreach (var effect in _effects)
        {
            ParticleSystem newEffect = Object.Instantiate(effect, Person.Transform);
            newEffect.transform.position = attack.ContactPoint;
            newEffect.transform.rotation = (attack.AttackDirection).GetRotation();

            PlayParticle(newEffect, attack.AttackCoificent.Clamp01());
            
            _debug.AttackCoificent = attack.AttackCoificent;
        }
    }

    private void PlayParticle(ParticleSystem particleSystem, float multiply)
    {
        particleSystem.transform.localScale *= multiply;
        particleSystem.Play();

        float timeAlive = particleSystem.main.duration * 10;//
        Object.Destroy(particleSystem, timeAlive);
    }

    [System.Serializable]
    private class CollisionEffectDebug
    {
        public float AttackCoificent;
    }
}