using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Joystick _joystick;
    [SerializeField] private AbilityButton _leftButton;
    [SerializeField] private AbilityButton _rightButton;

    private bool _isJoysticActive;

    private AbilityButton[] _buttons;

    private void Start()
    { 
        _buttons = new AbilityButton[]{ _leftButton, _rightButton };

        _joystick.OnDownEvent += OnJoystickDown;
        _joystick.OnUpEvent += OnJoystickUp;
    }

    private void Update()
    {
        if(_isJoysticActive == false && Input.GetMouseButtonDown(0))
        {
            if(HandleButton())
                return;
        }

        _joystick.CustomUpdate();
    }

    private bool HandleButton()
    {
        foreach (var button in _buttons)
        {
            if(IsPointInsideRect(button.Rect, Input.mousePosition))
            {
                button.OnClick();
                return true;
            }
        }
        return false;
    }

    private bool IsPointInsideRect(RectTransform rect, Vector2 point)
    {
        if (point.x < (rect.position.x + rect.rect.xMax)
            && point.x > (rect.position.x + rect.rect.xMin)
            && point.y < (rect.position.y + rect.rect.yMax)
            && point.y > (rect.position.y + rect.rect.yMin))
        {
            return true;
        }
        return false;
    }

    private void OnJoystickDown(Vector2 input)
    {
        foreach (var button in _buttons)
        {
            button.Hide();
        }
        _isJoysticActive = true;
    }

    private void OnJoystickUp(Vector2 input)
    {
        foreach (var button in _buttons)
        {
            button.Show();
        }
        _isJoysticActive = false;
    }
}