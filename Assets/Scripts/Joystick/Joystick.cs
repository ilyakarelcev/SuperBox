using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum MatchVariant
{
    Horizontal,
    Vertical
}

public enum InputType
{
    Mouse,
    Touch
}

/// <summary>
/// This joystick good work only with mouse. If you want use touch I recoomended you check all code on small error
/// </summary>

public class Joystick : MonoBehaviour
{

    [SerializeField] private RectTransform _backgroundTransform;
    [SerializeField] private RectTransform _stickTransform;
    [SerializeField] private RectTransform _defaultPosition;
    [Space]
    [SerializeField, Range(0, 1)] private float _size;
    [SerializeField, Range(0, 1)] private float _stickSize;
    [Space]
    [SerializeField, Range(0.1f, 1.5f)] private float _multiplySizeOnUnselected = 0.7f;
    [Space]
    [SerializeField] private RectTransform _canvasRectTransform;
    [SerializeField] private MatchVariant _matchVariant;
    [SerializeField] private RectTransform ActiveAreaRect;
    [SerializeField] private InputType _inputType;
    [Space]
    [SerializeField] private ChargeIndicationOnJoystic _chargeIndication;

    public Vector2 Value { get; private set; }
    public bool IsPressed { get; private set; }

    public event Action<Vector2> OnDownEvent;
    public event Action<Vector2> OnPressedEvent;
    public event Action<Vector2> OnUpEvent;

    [Space]
    public bool WriteInLog;

    private void OnValidate()
    {

        Vector2 backgroundSize;
        if (_matchVariant == MatchVariant.Horizontal)
        {
            backgroundSize = Vector2.one * _size * _canvasRectTransform.sizeDelta.x;
        }
        else
        {
            backgroundSize = Vector2.one * _size * _canvasRectTransform.sizeDelta.y;
        }

        SetSize(_size);
    }

    void Start()
    {
#if UNITY_ANDRIOD
        _inputType = InputType.Touch;
#endif
#if UNITY_IOS
        _inputType = InputType.Touch;
#endif

        SetSelection(false);
    }

    [SerializeField] private int _fingerId = -1;

    public void CustomUpdate()
    {

        if (_inputType == InputType.Touch)
        {
            GetTouchInput();
        }
        else
        {
            if (Input.GetMouseButtonDown(0)) OnDown(Input.mousePosition);
            if (Input.GetMouseButton(0)) OnPressed(Input.mousePosition);
            if (Input.GetMouseButtonUp(0)) OnUp(Input.mousePosition);
        }

    }

    bool IsPointInsideRect(RectTransform rect, Vector2 point)
    {
        if (point.x < (rect.position.x + rect.rect.xMax)
            && point.x > (rect.position.x + rect.rect.xMin)
            && point.y < (rect.position.y + rect.rect.yMax)
            && point.y > (rect.position.y + rect.rect.yMin))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void GetTouchInput()
    {
        foreach (Touch touch in Input.touches)
        {

            if (touch.phase == TouchPhase.Began)
            {
                if (_fingerId == -1)
                {
                    float relativeXPosition = touch.position.x / Screen.width;

                    if (WriteInLog)
                    {
                        Debug.Log(relativeXPosition);
                        Debug.Log(Time.time);
                    }

                    if (IsPointInsideRect(ActiveAreaRect, touch.position))
                    {
                        if (WriteInLog) Debug.Log("inside");

                        _fingerId = touch.fingerId;
                        OnDown(touch.position);
                        break;
                    }
                }

            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                if (touch.fingerId == _fingerId)
                {
                    OnPressed(touch.position);
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (touch.fingerId == _fingerId)
                {
                    _fingerId = -1;
                    OnUp(touch.position);
                }
            }
        }
    }

    public void OnDown(Vector2 touchPosition)
    {
        if (IsPressed)
            return;

        IsPressed = true;
        OnDownEvent?.Invoke(touchPosition);

        _backgroundTransform.position = touchPosition;
        SetSelection(true);
    }

    /* 
     * Со всем согласен, только странно каждый раз заново мерить радиус. Я бы его перещитывал его в OnValuate()
     */

    public void OnPressed(Vector2 touchPosition)
    {
        if (IsPressed == false)
            return;

        OnPressedEvent?.Invoke(touchPosition);

        Vector2 toMouse = touchPosition - (Vector2)_backgroundTransform.position;

        float distance = toMouse.magnitude;
        float pixelSize = _size * Screen.width;
        float radius = pixelSize * 0.5f;
        float toMouseClamped = Mathf.Clamp(distance, 0, radius);

        Vector2 stickPosition = toMouse.normalized * toMouseClamped;
        Value = stickPosition / radius;
        _stickTransform.localPosition = stickPosition;
    }

    public void OnUp(Vector2 touchPosition)
    {
        if (IsPressed == false)
            return;

        IsPressed = false;
        OnUpEvent?.Invoke(touchPosition);

        SetSelection(false);
        Value = Vector2.zero;
    }

    private void SetSelection(bool selectStatus)
    {
        if (selectStatus)
        {
            Show();
            return;

            SetSize(_size);
        }

        Hide();

        return;
        SetSize(_size * _multiplySizeOnUnselected);
        _backgroundTransform.position = _defaultPosition.position;
        _stickTransform.localPosition = Vector2.zero;
    }

    private void SetSize(float size)
    {
        Vector2 sizeVector = Vector2.one * size * Screen.width;

        _backgroundTransform.sizeDelta = sizeVector;
        _stickTransform.sizeDelta = sizeVector * _stickSize;

        _chargeIndication.ChangeSize(sizeVector);
    }

    private void Show()
    {
        _backgroundTransform.gameObject.SetActive(true);
        _stickTransform.gameObject.SetActive(true);
    }

    private void Hide()
    {
        _backgroundTransform.gameObject.SetActive(false);
        _stickTransform.gameObject.SetActive(false);
    }

}

public class JoystickSetup
{

}
