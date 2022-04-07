using Cephei.StateMachine;
using System;
using UnityEngine;

[Serializable]
public class AnimalBehaviour : StateMachineLevle, IPersonComponent
{
    [SerializeField] private GoToTargetPattern _backToHomePattern;
    [SerializeField] private IdlePattern _idlePattern;

    public IPerson Person { get; private set; }

    public void Init(IPerson person)
    {
        Person = person;

        _backToHomePattern.Transition.Add(new SimpleTransition(_idlePattern, _backToHomePattern, this));
        _backToHomePattern.Target = Person.Position;

        Person.InitializeThisComponents(_backToHomePattern, _idlePattern);

        Init(_backToHomePattern, new IStateMachinePattern[] { _idlePattern, _backToHomePattern });
    }
}
