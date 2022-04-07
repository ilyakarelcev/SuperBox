namespace Cephei.StateMachine
{
    public abstract class TransitionBase : ITransition
    {
        protected IStateMachinePattern _nextPattern;
        protected IStateMachine _stateMachine;

        public void Init(IStateMachinePattern nextPattern, IStateMachine stateMachine)
        {
            _nextPattern = nextPattern;
            _stateMachine = stateMachine;
        }

        public abstract void Activate();

        public abstract void DeActivate();

        protected void ActivatePattern()
        {
            _stateMachine.ActivatePattern(_nextPattern);
        }
    }
}
