using UnityEngine;

[System.Serializable]
public class SoundAttackTakerNew : SoundSource, IAttackTaker
{
    public void TakeAttack(Attack attack)
    {
        float volume = _soundSetup.Volume * attack.AttackCoificent;
        SoundSetup setup = new SoundSetup(volume, _soundSetup.Pitch, _soundSetup.Clip);
        Sound.SetupSource(setup, _source);
        _source.Play();
    }
}
