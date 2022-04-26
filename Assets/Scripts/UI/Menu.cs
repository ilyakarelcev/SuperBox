using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameFlow _gameFlow;
    [SerializeField] private GameObject _menu;
    [Space]
    [SerializeField] private Button _openButton;

    private void Start()
    {
        _menu.SetActive(false);
    }
    
    [ContextMenu("Open")]
    public void Open()
    {
        _menu.SetActive(true);

        _openButton.gameObject.SetActive(false);

        _gameFlow.Pause();
    }

    [ContextMenu("Close")]
    public void Close()
    {
        _menu.SetActive(false);
        _openButton.gameObject.SetActive(true);

        _gameFlow.Unpause();
    }

    [ContextMenu("Restart")]
    public void Restart()
    {
        _gameFlow.Unpause();
        _gameFlow.Restart();
    }
}
