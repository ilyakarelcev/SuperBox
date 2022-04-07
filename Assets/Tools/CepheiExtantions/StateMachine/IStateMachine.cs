namespace Cephei.StateMachine
{
    public interface IStateMachine
    {
        IStateMachinePattern CurentPattern { get; }

        void StartWork(IStateMachinePattern statPattern);

        void StopWork();

        void Continue();

        void ActivatePattern(IStateMachinePattern nextPattern); 

        T GetPatternByType<T>() where T : IStateMachinePattern;

        bool GetPatternBy<T>(out T pattern) where T : IStateMachinePattern;

        T GetPatternIs<T>() where T : IStateMachinePattern;

        bool GetPatternIs<T>(out T pattern) where T : IStateMachinePattern;
    }
}
