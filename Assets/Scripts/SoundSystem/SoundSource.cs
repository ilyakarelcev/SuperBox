using UnityEngine;

public class SoundSource
{
    [SerializeField] protected AudioSource _source;

    protected SoundSetup _soundSetup;

    public void Init(SoundSetup setup)
    {
        _soundSetup = setup;
        Sound.AddSource(_source);
    }

    protected void PlaySetup(SoundSetup setup)
    {
        Sound.SetupSource(setup, _source);
        _source.Play();
    }
}
