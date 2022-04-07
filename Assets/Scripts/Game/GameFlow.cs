using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFlow : MonoBehaviour
{
    [SerializeField] private int _duration = 180;
    [SerializeField] private Text _timeText;

    private IEnumerator Start()
    {
        float timer = 0;
        int passedTime = 0;
        while (true)
        {
            if(timer > 1)
            {
                passedTime += 1;
                timer -= 1;

                ShowClock(_duration - passedTime);
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
