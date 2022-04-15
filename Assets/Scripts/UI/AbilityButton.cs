using Cephei;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private IAbility _ability;

    private bool _swipeControle;
    private float _swipeMagnitude = 500;

    private Vector2 _downPointerPosition;

    [Space]
    public bool Log;

    public void Setup(IAbility ability, bool swipeControl, float swipeMagnitude)
    {
        _swipeControle = swipeControl;
        _swipeMagnitude = swipeMagnitude;

        _ability = ability;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _downPointerPosition = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_swipeControle == false)
            UseAbility();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(_swipeControle && _downPointerPosition != Vector2.zero)
        {
            if ((eventData.position - _downPointerPosition).sqrMagnitude > _swipeMagnitude.Sqr())
            {
                UseAbility();
                _downPointerPosition = Vector2.zero;
            }
        }
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

    public void UseAbility()
    {
        _ability.Use();
    }
}
