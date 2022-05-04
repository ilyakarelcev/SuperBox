using System;
using UnityEngine;
using UnityEngine.EventSystems;

public enum MatchVariant
{
    Horizontal,
    Vertical
}

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform _backgroundTransform;
    [SerializeField] private RectTransform _stickTransform;
    [Space]
    [SerializeField, Range(0, 1)] private float _size;
    [SerializeField, Range(0, 1)] private float _stickSize;
    [Space]
    [SerializeField] private RectTransform _canvasRectTransform;
    [SerializeField] private MatchVariant _matchVariant;
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
        UpdateSize();
    }

    void Start()
    {
        UpdateSize();
        SetSelection(false);
    }

    private void Update()
    {
        if (_currentPosition != Vector2.zero)
            OnPressed(_currentPosition);
    }

    private Vector2 _currentPosition;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsPressed)
            return;

        IsPressed = true;
        _currentPosition = eventData.position;

        OnDownEvent?.Invoke(eventData.position);

        _backgroundTransform.position = eventData.position;
        SetSelection(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _currentPosition = eventData.position;
    }

    public void OnPressed(Vector2 touchPosition)
    {
        if (IsPressed == false)
            return;

        OnPressedEvent?.Invoke(touchPosition);

        Vector2 toMouse = touchPosition - (Vector2)_backgroundTransform.position;

        float distance = GetMatchedFloat(toMouse.magnitude / GetMatchedScreenSize());
        float pixelSize = GetMatchedFloat(_size);

        float radius = pixelSize * 0.5f;

        float toMouseClamped = Mathf.Clamp(distance, 0, radius);

        Vector2 stickPosition = toMouse.normalized * toMouseClamped;
        Value = stickPosition / radius;
        _stickTransform.localPosition = stickPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (IsPressed == false)
            return;

        IsPressed = false;
        OnUpEvent?.Invoke(eventData.position);

        SetSelection(false);
        Value = Vector2.zero;
        _currentPosition = Vector2.zero;
    }

    private float GetMatchedFloat(float f)
    {
        if (_matchVariant == MatchVariant.Horizontal)
            return f * _canvasRectTransform.sizeDelta.x;
        else
            return f * _canvasRectTransform.sizeDelta.y;
    }

    private float GetMatchedScreenSize()
    {
        if (_matchVariant == MatchVariant.Horizontal)
            return Screen.width;
        else
            return Screen.height;
    }

    private void SetSelection(bool selectStatus)
    {
        if (selectStatus)
            Show();
        else
            Hide();
    }

    private void UpdateSize()
    {
        float currentSize = GetMatchedFloat(_size);

        Vector2 sizeVector = Vector2.one * currentSize;

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
