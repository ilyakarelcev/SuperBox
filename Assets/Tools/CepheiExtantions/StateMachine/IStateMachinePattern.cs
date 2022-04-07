using System;
using System.Collections.Generic;

namespace Cephei.StateMachine
{
    public interface IStateMachinePattern
    {
        event Action EndWorkEvent;

        List<ITransition> Transition { get; }

        bool IsActive { get; }

        void Activate();

        void DeActivate();
    }
}