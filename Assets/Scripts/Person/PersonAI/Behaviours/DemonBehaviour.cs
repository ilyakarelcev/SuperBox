using Cephei.StateMachine;
using System;
using UnityEngine;

[System.Serializable]
public class DemonBehaviour : StateMachineLevle, IPersonComponent
{
    [SerializeField] private Transform _castlePoint;

    [SerializeField] private GoToTargetPattern _goToTargetPattern;

    public IPerson Person { get; private set; }

    public event Action OnComeToCastle;

    public void Init(IPerson person)
    {
        Person = person;

        _goToTargetPattern.EndWorkEvent += () => OnComeToCastle?.Invoke();
        _goToTargetPattern.Target = _castlePoint.position;

        person.InitializeThisComponents(_goToTargetPattern);

        Init(_goToTargetPattern, _goToTargetPattern);
    }
}