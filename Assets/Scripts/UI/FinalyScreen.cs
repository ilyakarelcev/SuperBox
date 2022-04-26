using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalyScreen : MonoBehaviour
{
    [SerializeField] private float _delay = 5;
    [Space]
    [SerializeField] private GameFlow _gameFlow;
    [SerializeField] private Score _score;
    [Space]
    [SerializeField] private GameObject _screen;
    [SerializeField] private Text _scoreText;

    private IEnumerator Start()
    {
        yield return null;

        _gameFlow.TimeEndEvent += ShowScreen;
        PlayerStaticInfo.Player.HealthManager.DieEvent += Delay;
    }

    public void Restart()
    {
        _gameFlow.Unpause();
        _gameFlow.Restart();
    }

    private void Delay()
    {
        Invoke(nameof(ShowScreen), _delay);
    }

    private void ShowScreen()
    {
        _gameFlow.Pause();

        _screen.SetActive(true);
        _scoreText.text = _score.CurrentScore.ToString() + " / " + _score.AllScore.ToString();
    }
}
