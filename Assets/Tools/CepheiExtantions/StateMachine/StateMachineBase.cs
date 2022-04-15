using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cephei.StateMachine
{
    public abstract class StateMachineBase : IStateMachine
    {
        public IStateMachinePattern CurentPattern { get; private set; }

        protected IStateMachinePattern[] _patterns;

        public void Init(params IStateMachinePattern[] patterns)
        {
            _patterns = patterns;
        }

        public void StartWork(IStateMachinePattern startPattern)
        {
            CurentPattern = startPattern;
            startPattern.Activate();
        }

        public void StopWork()
        {
            CurentPattern.DeActivate();
        }

        public void Continue()
        {
            CurentPattern.Activate();
        }

        public void ActivatePattern(IStateMachinePattern nextPattern)
        {
            CurentPattern.DeActivate();
            CurentPattern = nextPattern;
            CurentPattern.Activate();
        }

        public T GetPatternByType<T>() where T : IStateMachinePattern
        {
            foreach (var pattern in _patterns)
            {
                if (pattern.GetType() == typeof(T))
                    return (T)pattern;
            }
            return default;
        }

        public bool GetPatternBy<T>(out T pattern) where T : IStateMachinePattern
        {
            foreach (var patternlLocal in _patterns)
            {
                if (patternlLocal.GetType() == typeof(T))
                {
                    pattern = (T)patternlLocal;
                    return true;
                }
            }
            pattern = default;
            return false;
        }

        public T GetPatternIs<T>() where T : IStateMachinePattern
        {
            foreach (var pattern in _patterns)
            {
                if (pattern is T)
                    return (T)pattern;
            }
            return default;
        }

        public bool GetPatternIs<T>(out T pattern) where T : IStateMachinePattern
        {
            foreach (var patternlLocal in _patterns)
            {
                if (patternlLocal is T)
                {
                    pattern = (T)patternlLocal;
                    return true;
                }
            }
            pattern = default;
            return false;
        }
    }
}
