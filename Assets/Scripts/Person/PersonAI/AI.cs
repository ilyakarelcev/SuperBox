using Cephei.StateMachine;
using UnityEngine;

[System.Serializable]
public class AITypeOne : StateMachineBase
{
    [SerializeField] private GoToTargetPattern _goToTargetPattern;
    private StateMachineLevle _attackPattern;

    public void Init()
    {
        _patterns = new IStateMachinePattern[]
        {
            _goToTargetPattern,
            _attackPattern
        };
    }

    public void InitAttackPattern()
    {

    }
}
