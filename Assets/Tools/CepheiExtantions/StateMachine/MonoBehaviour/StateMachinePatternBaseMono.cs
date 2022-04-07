using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cephei.StateMachine.MonoBehaviour
{
    public abstract class StateMachinePatternBaseMono : UnityEngine.MonoBehaviour, IStateMachinePattern
    {
        [SerializeField] private TransitionBaseMono[] _transitions;

        public List<ITransition> Transition { get; private set; }

        public bool IsActive { get; private set; }

        public event Action EndWorkEvent;

        public virtual void Activate()
        {
            IsActive = true;

            foreach (var transition in _transitions)
            {
                transition.Activate();
            }
        }

        public virtual void DeActivate()
        {
            IsActive = false;

            foreach (var transition in _transitions)
            {
                transition.DeActivate();
            }
        }
    }
}