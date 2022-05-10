using UnityEngine;

[System.Serializable]
public class SoundAttackTaker : SoundSource, IAttackTaker
{
    public void TakeAttack(Attack attack)
    {
        float volume = _soundSetup.Volume * attack.AttackCoificent;
        SoundSetup setup = new SoundSetup(volume, _soundSetup.Pitch, _soundSetup.Clip);
        Sound.SetupSource(setup, _source);
        _source.Play();

        return;

        //_source.volume = _startVolume * attack.AttackCoificent;
        //_source.Play();
    }
}