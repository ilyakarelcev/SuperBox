using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Joystick _joystick;
    [Space]
    [SerializeField] private AbilityButton[] _buttons;

    private void Start()
    {
        _joystick.OnDownEvent += (p) => HideButton();
        _joystick.OnUpEvent += (p) => ShowButton();
    }    

    private void ShowButton()
    {
        foreach (var button in _buttons)
        {
            button.Show();
        }
    }

    private void HideButton()
    {
        foreach (var button in _buttons)
        {
            button.Hide();
        }
    }    
}