using Cephei.StateMachine;

[System.Serializable]
public class TransitionOnWallCollision : TransitionBase
{
    private WallChecker _wallChecker;

    public void Init(IStateMachinePattern nextPattern, IStateMachine stateMachine, WallChecker wallChecker)
    {
        _nextPattern = nextPattern;
        _stateMachine = stateMachine;

        _wallChecker = wallChecker;
    }

    public override void Activate()
    {
        _wallChecker.OnDetectWall += OnDetectWall;
    }

    public override void DeActivate()
    {
        _wallChecker.OnDetectWall -= OnDetectWall;
    }

    private void OnDetectWall()
    {
        ActivatePattern();
    }
}
