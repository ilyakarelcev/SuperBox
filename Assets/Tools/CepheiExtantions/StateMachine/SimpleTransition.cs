namespace Cephei.StateMachine
{
    public class SimpleTransition : TransitionBase
    {
        protected IStateMachinePattern _selfPattern;

        public SimpleTransition(IStateMachinePattern nextPattern, IStateMachinePattern selfPattern, 
            IStateMachine stateMachine)
        {
            _selfPattern = selfPattern;
            Init(nextPattern, stateMachine);
        }

        public override void Activate()
        {
            _selfPattern.EndWorkEvent += ActivatePattern;
        }

        public override void DeActivate()
        {
            _selfPattern.EndWorkEvent -= ActivatePattern;
        }
    }
}
