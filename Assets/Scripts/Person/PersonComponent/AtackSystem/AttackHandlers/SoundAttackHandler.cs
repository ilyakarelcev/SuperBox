using UnityEngine;

[System.Serializable]
public class SoundAttackHandler : IAttackHandler, IPersonComponent
{
    [SerializeField] private AudioSource _source;

    public IPerson Person { get; private set; }

    private float _startVolume;

    public void Init(IPerson person)
    {
        Person = person;
        _startVolume = _source.volume;
    }

    public void Handle(Attack attack)
    {
        _source.volume = _startVolume * attack.AttackMultiply;
        _source.Play();
    }
}