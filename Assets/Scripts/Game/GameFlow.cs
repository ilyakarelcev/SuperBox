using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameFlow : MonoBehaviour
{
    [SerializeField] private int _duration = 180;
    [SerializeField] private Text _timeText;

    public event Action TimeEndEvent;

    [ContextMenu("Pause")]
    public void Pause()
    {
        TimeScaleManager.SetTimeScale(0);
    }

    [ContextMenu("UnPause")]
    public void Unpause()
    {
        TimeScaleManager.SetTimeScale(1);
    }

    [ContextMenu("Restart")]
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator Start()
    {
        Debug.Log("Time Scale on Start: " + Time.timeScale);
        ShowClock(_duration);

        float timer = 0;
        int passedTime = 0;
        while (true)
        {
            if(timer > 1)
            {
                passedTime += 1;
                timer -= 1;

                ShowClock(_duration - passedTime);                

                if (passedTime == _duration)
                {
                    TimeEndEvent?.Invoke();
                    yield break;
                }
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }

    private void ShowClock(int time)
    {
        int minute = Mathf.FloorToInt(time / 60);
        int second = time % 60;
        _timeText.text = minute.ToString("00") + ":" + second.ToString("00");
    }
}
