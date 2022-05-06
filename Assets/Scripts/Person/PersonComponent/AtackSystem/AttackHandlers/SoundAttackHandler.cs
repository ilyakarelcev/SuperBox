using UnityEngine;

[System.Serializable]
public class SoundAttackHandler : SoundSource, IAttackHandler
{
    public void Handle(Attack attack)
    {
        float volume = _soundSetup.Volume * attack.AttackMultiply;
        SoundSetup setup = new SoundSetup(volume, _soundSetup.Pitch, _soundSetup.Clip);
        PlaySetup(setup);

        return;

        //_source.volume = _startVolume * attack.AttackMultiply;
        //_source.Play();
    }
}