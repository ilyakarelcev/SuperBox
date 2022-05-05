using UnityEngine;

[System.Serializable]
public class SoundAttackTakerNew : IAttackTaker
{
    [SerializeField] private AudioSource _source;

    private SoundSetup _soundSetup;

    public void Init(SoundSetup setup)
    {
        _soundSetup = setup;
        Sound.AddSource(_source);
    }

    public void TakeAttack(Attack attack)
    {
        float volume = _soundSetup.Volume * attack.AttackCoificent;
        SoundSetup setup = new SoundSetup(volume, _soundSetup.Pitch, _soundSetup.Clip);
        Sound.SetupSource(setup, _source);
        _source.Play();
    }

    public void Test()
    {
        float volume = _soundSetup.Volume * 1;
        SoundSetup setup = new SoundSetup(volume, _soundSetup.Pitch, _soundSetup.Clip);
        Sound.SetupSource(setup, _source);
        _source.Play();        
    }
}
