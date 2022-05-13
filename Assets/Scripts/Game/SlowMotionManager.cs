using Cephei;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionManager : MonoBehaviour, ISengleTone
{
    public event Action BeginWorkEvent;
    public event Action EndWorkEvent;

    public bool IsWork { get; private set; }
    public float CurrentScale { get; private set; }

    public static SlowMotionManager Instance;

    private LinkedList<Operation> _operations = new LinkedList<Operation>();

    [Header("View")]
    public float TimeScaleView;

    public void Init()
    {
        if (Instance)
            Debug.Log("SengleTone exaption. More one instance");

        Instance = this;
        enabled = false;
    }

    void Update()
    {
        TimeScaleView = Time.timeScale;      

        float minScale = Mathf.Infinity;
        LinkedListNode<Operation> curentNode = _operations.First;

        while (curentNode != null)
        {
            LinkedListNode<Operation> nextNode = curentNode.Next;

            if (curentNode.Value.GetTimeScale(Time.unscaledDeltaTime, out float timeScale))
                _operations.Remove(curentNode);
            curentNode = nextNode;

            minScale = Mathf.Min(minScale, timeScale);
        }

        CurrentScale = minScale;
        TimeScaleManager.SetTimeScale(minScale);

        if (_operations.Count == 0)
            EndWork();
    }

    public Operation AddOperation(float duration, Func<float, float> func)
    {
        if (IsWork == false)
            BeginWork();

        _operations.AddLast(new Operation() { Duration = duration, Func = func });
        return _operations.Last.Value;
    }

    public bool RemoveOperation(Operation operation)
    {
        return _operations.Remove(operation);
    }

    private void BeginWork()
    {
        IsWork = true;
        enabled = true;

        BeginWorkEvent?.Invoke();
    }

    private void EndWork()
    {
        IsWork = false;
        enabled = false;

        EndWorkEvent?.Invoke();
    }

    public class Operation
    {
        public float Duration;
        public float CurrentTime;
        public Func<float, float> Func;

        public bool GetTimeScale(float delta, out float timeScale)
        {
            CurrentTime += delta;
            float percent = (CurrentTime / Duration).Clamp01();

            timeScale = Func.Invoke(percent);

            if (percent >= 1) 
                return true;

            return false;
        }
    }
}
