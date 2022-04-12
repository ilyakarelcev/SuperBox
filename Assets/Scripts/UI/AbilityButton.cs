using Cephei;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityButton : MonoBehaviour
{
    [SerializeField] private RectTransform _area;
    [SerializeField] private IAbility _ability;

    public RectTransform Rect => _area;
    public event Action ClicEvent;
    public event Action EndTouch;

    private bool _swipeControle;
    private float _swipeMagnitude = 500;

    [Space]
    public bool Log;
    [SerializeField] private AbilityButtonDebug _debug;

    public void Setup(IAbility ability, bool swipeControl, float swipeMagnitude)
    {
        _swipeControle = swipeControl;
        _swipeMagnitude = swipeMagnitude;

        _ability = ability;
    }

    public bool GetTouch(ITouchUnit touchUnit)
    {
        if (UISuporter.IsPointUnsideRect(touchUnit.StartPosition, Rect))
        {
            if (_swipeControle)
                touchUnit.PositionChangeEvent += OnPositionChange;

            touchUnit.EndTouchEvent += OnEndOfTouch;
            return true;
        }
        return false;
    }

    public void Show()
    {
        if (Log) Debug.Log("ShowButton");
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        if (Log) Debug.Log("HideButton");
        gameObject.SetActive(false);
    }

    public void OnClick()
    {
        _ability.Use();
    }

    private void OnPositionChange(ITouchUnit touchUnit)
    {
        _debug.GetTouch(touchUnit);
    }

    private void OnEndOfTouch(ITouchUnit touchUnit)
    {
        if (_swipeControle)
        {
            float swipeMagnitude = (touchUnit.StartPosition - touchUnit.CurentPosition).sqrMagnitude;
            if (swipeMagnitude > _swipeMagnitude.Sqr())
                OnClick();
            if (Log) Debug.Log("Swipe mag: " + swipeMagnitude);
        }
        else
            OnClick();

        EndTouch?.Invoke();
    }

    [System.Serializable]
    private class AbilityButtonDebug
    {
        public Vector2 Swipe;
        public float SwipeMagnitude;

        public void GetTouch(ITouchUnit touchUnit)
        {
            Swipe = touchUnit.Swipe;
            SwipeMagnitude = Swipe.magnitude;
        }
    }
}
