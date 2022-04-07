using System;
using System.Collections.Generic;

namespace Cephei.StateMachine
{
    public class StateMachineLevle : StateMachineBase, IStateMachinePattern
    {
        public event Action EndWorkEvent;

        private IStateMachinePattern _startPattern;

        public List<ITransition> Transition { get; set; } = new List<ITransition>();

        public bool IsActive { get; private set; }

        public void Init(IStateMachinePattern startPattern, params IStateMachinePattern[] patterns)
        {
            _patterns = patterns;
            _startPattern = startPattern;
        }

        public void Activate()
        {
            IsActive = true;
            foreach (var transition in Transition)
            {
                transition.Activate();
            }

            StartWork(_startPattern);
        }

        public void DeActivate()
        {
            IsActive = false;
            foreach (var transition in Transition)
            {
                transition.DeActivate();
            }

            StopWork();
        }

        protected void InvokeEndWorkEvent()
        {
            EndWorkEvent.Invoke();
        }
    }
}