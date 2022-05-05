using UnityEngine;

public class TestSound : MonoBehaviour
{
    public SoundAttackTakerNew taker;

    public bool Test;

    private void Start()
    {
        taker.Init(Sound.Bank.Hit);

        
        taker.Test();
    }

    private void Update()
    {
        if (Test)
        {
            Test = false;
            taker.Test();
        }
    }
}