using UnityEngine;

public interface IStatic
{

}

public class TestSound : MonoBehaviour
{
    public SoundAttackTakerNew taker;

    public bool Test;

    private void Start()
    {
        return;

        taker.Init(Sound.Bank.Hit);
    }

    private void Update()
    {
        if (Test)
        {
            Test = false;
        }

        if (Play)
        {
            Play = false;
            _source.Play();
        }
        if (Puse)
        {
            _source.Pause();
            Puse = false;
        }
        if (UnPause)
        {
            UnPause = false;
            _source.UnPause();
        }
    }

    public bool Play;
    public bool Puse;
    public bool UnPause;

    public AudioSource _source;
}