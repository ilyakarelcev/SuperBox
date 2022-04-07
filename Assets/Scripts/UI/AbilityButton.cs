using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityButton : MonoBehaviour
{
    [SerializeField] private RectTransform _area;

    public RectTransform Rect => _area;
    public event Action ClicEvent;

    public void Show()
    {
        Debug.Log("ShowButton");
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        Debug.Log("HideButton");
        gameObject.SetActive(false);
    }

    public void OnClick()
    {
        Debug.Log("Click on Button");
        ClicEvent?.Invoke();
    }
}
