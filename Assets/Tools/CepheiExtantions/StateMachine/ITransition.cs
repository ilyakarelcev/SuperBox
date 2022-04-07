namespace Cephei.StateMachine
{
    public interface ITransition
    {
        public abstract void Activate();

        public abstract void DeActivate();
    }
}