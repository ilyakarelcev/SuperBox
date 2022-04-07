using UnityEngine;

namespace Cephei.StateMachine.MonoBehaviour
{
    public abstract class TransitionBaseMono : UnityEngine.MonoBehaviour, ITransition
    {
        [SerializeField] protected StateMachinePatternBaseMono _nextPattern;
        [SerializeField] protected StateMachineMono _stateMachine;

        public abstract void Activate();

        public abstract void DeActivate();

        protected void ActivePattern()
        {
            _stateMachine.ActivatePattern(_nextPattern);
        }
    }
}