using System;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineOperator
{
    private LinkedList<UpdateCoroutine> _updateCoroutines = new LinkedList<UpdateCoroutine>();
    private LinkedList<UpdateCoroutine> _fixedUpdateCoroutines = new LinkedList<UpdateCoroutine>();
    private LinkedList<UpdateCoroutine> _lateUpdateCoroutines = new LinkedList<UpdateCoroutine>();
    private LinkedList<CoroutineWithTimeStep> _timeStepCoroutines = new LinkedList<CoroutineWithTimeStep>();

    public void RunUpdate()
    {
        ByPassList(_updateCoroutines, x => x.Invoke());

        float deltaTime = Time.deltaTime;
        ByPassList(_timeStepCoroutines, x => x.UpdateTimer(deltaTime));
    }

    public void RunFixedUpdate()
    {
        ByPassList(_fixedUpdateCoroutines, x => x.Invoke());
    }

    public void RunLateUpdate()
    {
        ByPassList(_lateUpdateCoroutines, x => x.Invoke());
    }

    public CastomCoroutine OpenUpdateCoroutine(Action action, LifeType lifeType, UpdateType updateType = UpdateType.Update)
    {
        LinkedList<UpdateCoroutine> list;
        if (updateType == UpdateType.Update)
            list = _updateCoroutines;
        else if (updateType == UpdateType.FixedUpdate)
            list = _fixedUpdateCoroutines;
        else
            list = _lateUpdateCoroutines;

        return InitCoroutine(new UpdateCoroutine(action, lifeType), list);
    }

    public CastomCoroutine OpenCoroutineWithTimeStep(Action action, float timeStep, LifeType lifeType)
    {
        return InitCoroutine(new CoroutineWithTimeStep(action, lifeType, timeStep), _timeStepCoroutines);
    }

    private CastomCoroutine InitCoroutine<T>(SmartCoroutine coroutine, LinkedList<T> list) where T : SmartCoroutine
    {
        coroutine.OnDestroy += (x) => list.Remove(x as T);
        list.AddLast(coroutine as T);

        return coroutine;
    }

    private void ByPassList<T>(LinkedList<T> list, Action<T> action)
    {
        LinkedListNode<T> nowNode = list.First;
        while (nowNode != null)
        {
            action.Invoke(nowNode.Value);
            LinkedListNode<T> nextNode = nowNode.Next;
            nowNode = nextNode;
        }
    }

    private abstract class SmartCoroutine : CastomCoroutine
    {
        public Action<SmartCoroutine> OnDestroy;

        public SmartCoroutine(Action action, LifeType lifeType)
            : base(action, lifeType) { }

        public override void Destroy()
        {
            OnDestroy.Invoke(this);
        }
    }

    private class UpdateCoroutine : SmartCoroutine
    {
        public UpdateCoroutine(Action action, LifeType lifeType)
            : base(action, lifeType) { }

        public void Invoke()
        {
            if (IsPused) return;

            InvokeAction();
        }
    }

    private class CoroutineWithTimeStep : SmartCoroutine
    {
        private float _timeStep;
        private float _timer;

        public CoroutineWithTimeStep(Action action, LifeType lifeType, float timeStep)
            : base(action, lifeType)
        {
            _timeStep = timeStep;
        }

        public void UpdateTimer(float delteTime)
        {
            if (IsPused) return;

            _timer += delteTime;

            if (_timer > _timeStep)
            {
                InvokeAction();
                _timer -= _timeStep;
            }
        }
    }
}

