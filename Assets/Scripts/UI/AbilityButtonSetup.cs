using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityButtonSetup : MonoBehaviour
{
    [SerializeField] private bool _swipeControl;
    [SerializeField, Range(0, 1)] private float _swipeMagnitudePrcentAtScrean = 0.1f;
    [SerializeField] private MatchVariant _matchVariant;
    [Space]
    [SerializeField] private AbilityButton _left;
    [SerializeField] private AbilityButton _right;
    [Space]
    [SerializeField] private GameObject _leftAbility;
    [SerializeField] private GameObject _rightAbility;

    [SerializeField] private float _swipeMagnitude;

    [Space]
    public Vector2 ScreenSizeView;

    private void OnValidate()
    {
        CalculateSwipeMagnitude();

        if(Application.isPlaying)
            SetupButtons(_swipeControl);
    }

    private void Start()
    {
        CalculateSwipeMagnitude();

        SetupButtons(_swipeControl);
    }

    public void OnToggleChange(bool value)
    {
        SetupButtons(value);
    }

    private void SetupButtons(bool value)
    {
        _left.Setup(_leftAbility.GetComponent<IAbility>(), value, _swipeMagnitude);
        _right.Setup(_rightAbility.GetComponent<IAbility>(), value, _swipeMagnitude);
    }

    private void CalculateSwipeMagnitude()
    {
        if (_matchVariant == MatchVariant.Horizontal)
            _swipeMagnitude = Screen.width * _swipeMagnitudePrcentAtScrean;
        else if (_matchVariant == MatchVariant.Vertical)
            _swipeMagnitude = Screen.height * _swipeMagnitudePrcentAtScrean;

        ScreenSizeView = new Vector2(Screen.width, Screen.height);
    }
}