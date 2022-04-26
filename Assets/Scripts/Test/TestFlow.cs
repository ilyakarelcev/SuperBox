using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFlow : MonoBehaviour
{
    public GameFlow _flow;

    public bool TestPause;
    public bool TestUnPause;
    public bool TestRestart;

    private void Update()
    {
        if (TestPause)
        {
            _flow.Pause();
            TestPause = false;
        }
        if (TestUnPause)
        {
            _flow.Unpause();
            TestUnPause = false;
        }
        if (TestRestart)
        {
            _flow.Restart();

            TestRestart = false;
        }
    }

    public void Restart()
    {
        _flow.Restart();
    }
}
