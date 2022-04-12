using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Joystick _joystick;
    [SerializeField] private AbilityButton _leftButton;
    [SerializeField] private AbilityButton _rightButton;
    [Space]
    [SerializeField] private InputConteiner _inputConteiner;
    private bool _touchIsUsed;

    [SerializeField] private AbilityButton[] _buttons;

    private void Start()
    {
        _inputConteiner.TouchEvent += OnTouch;

        //_buttons = new AbilityButton[] { _leftButton, _rightButton };

        foreach (var button in _buttons)
        {
            button.EndTouch += OnButtonEndTouch;
        }
        _joystick.OnUpEvent += OnJoystickUp;
    }

    private void OnTouch(ITouchUnit touchUnit)
    {
        if (_touchIsUsed) return;

        foreach(var button in _buttons)
        {
            if (button.GetTouch(touchUnit))
            {
                _touchIsUsed = true;
                return;
            }
        }

        if (_joystick.GetTouch(touchUnit))
        {
            HideButton();
            _touchIsUsed = true;
        }
    }

    private void OnButtonEndTouch()
    {
        _touchIsUsed = false;
    }    

    private void OnJoystickUp(Vector2 input)
    {
        ShowButton();
        _touchIsUsed = false;
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