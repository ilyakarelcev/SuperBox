using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cephei.StateMachine.MonoBehaviour
{
    public class StateMachineMono : UnityEngine.MonoBehaviour, IStateMachine
    {
        [SerializeField] protected StateMachinePatternBase[] _patternsMono;
        [SerializeField] protected StateMachinePatternBaseMono _startPattern;

        protected IStateMachinePattern[] _patterns;

        public event Action<IStateMachinePattern> ActivatePatternEvent; //Don't use

        public IStateMachinePattern CurentPattern { get; private set; }

        public void StartWork(IStateMachinePattern startPattern)
        {
            ActivatePattern(startPattern);
            CurentPattern = startPattern;
        }

        public void StartWork()
        {
            ActivatePattern(_startPattern);
            CurentPattern = _startPattern;
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


        // Pattern Geters


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