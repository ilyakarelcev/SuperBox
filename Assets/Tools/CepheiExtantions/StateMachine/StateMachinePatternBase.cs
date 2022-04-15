using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cephei.StateMachine
{
    public abstract class StateMachinePatternBase : IStateMachinePattern
    {
        public List<ITransition> Transition { get; set; } = new List<ITransition>();

        public bool IsActive { get; private set; }

        public event Action EndWorkEvent;

        public virtual void Activate()
        {
            IsActive = true;

            foreach (var transition in Transition)
            {
                transition.Activate();
            }
        }

        public virtual void DeActivate()
        {
            IsActive = false;

            foreach (var transition in Transition)
            {
                transition.DeActivate();
            }
        }

        protected void InvokeEndWorkEvent() => EndWorkEvent?.Invoke();


        public void LogAllEndWorkEventSubscribers()
        {
            Debug.Log("Begin Log " + this.ToString());
            if(EndWorkEvent == null)
            {
                Debug.Log("Event is null");
                return;
            }

            foreach (var item in EndWorkEvent.GetInvocationList())
            {
                Debug.Log("Method name: " + item.Method.Name);
            }
            Debug.Log("End Log");
        }
    }
}