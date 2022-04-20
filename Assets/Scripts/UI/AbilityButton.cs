using Cephei;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Animator _triggerAnimator;
    [Space]
    [SerializeField] private RectTransform _insideCircle;

    private IAbility _ability;

    private bool _swipeControle;
    private float _swipeMagnitude = 500;

    private Vector2 _downPointerPosition;

    [Space]
    public bool Log;

    public bool IsTesting;

    public void Update()
    {
        if (IsTesting == false) return;

        if (Input.GetKeyDown(KeyCode.S))
        {
            Show();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Hide();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            _triggerAnimator.SetTrigger("Trigger");
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            _triggerAnimator.SetTrigger("NotFullCoolDown");
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            UseAbility();
        }
    }

    public void Setup(IAbility ability, bool swipeControl, float swipeMagnitude)
    {
        _swipeControle = swipeControl;
        _swipeMagnitude = swipeMagnitude;

        _ability = ability;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_ability.CanUse)
            _downPointerPosition = eventData.position;
        else
            _triggerAnimator.SetTrigger("NotFullCoolDown");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_swipeControle == false && _ability.CanUse)
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

        _animator.SetTrigger("Show");
    }

    public void Hide()
    {
        if (Log) Debug.Log("HideButton");

        _animator.SetTrigger("Hide");
    }

    public void UseAbility()
    {
        _ability.Use();

        _triggerAnimator.SetTrigger("Trigger");
        StartCoroutine(IndicateCoolDownPercent());
    }

    private IEnumerator IndicateCoolDownPercent()
    {
        while (_ability.CoolDownPercent < 1)
        {
            _insideCircle.localScale = Vector3.one * _ability.CoolDownPercent;
            yield return null;
        }
                
        _triggerAnimator.SetTrigger("Trigger");
    }
}
