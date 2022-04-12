using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityButtonSetup : MonoBehaviour
{
    [SerializeField] private bool _swipeControl;
    [SerializeField] private float _swipeMagnitude = 350;
    [Space]
    [SerializeField] private AbilityButton _left;
    [SerializeField] private AbilityButton _right;
    [Space]
    [SerializeField] private GameObject _leftAbility;
    [SerializeField] private GameObject _rightAbility;

    private void Start()
    {
        _left.Setup(_leftAbility.GetComponent<IAbility>(), _swipeControl, _swipeMagnitude);
        _right.Setup(_rightAbility.GetComponent<IAbility>(), _swipeControl, _swipeMagnitude);
    }
}