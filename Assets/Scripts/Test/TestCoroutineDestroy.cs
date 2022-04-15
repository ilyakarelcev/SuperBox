using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCoroutineDestroy : MonoBehaviour
{
    private CoroutineOperator Operator;

    private CastomCoroutine CastomCoroutine;

    private void Start()
    {
        Operator = new CoroutineOperator();
        CastomCoroutine = Operator.OpenCoroutineWithTimeStep(SomeMethod, 1, LifeType.Cycle);
    }

    private void SomeMethod()
    {
        Debug.LogError("Some message");
    }

    protected void Update()
    {
        CustomUpdate();
        CastomCoroutine.Destroy();
    }

    private void FixedUpdate()
    {
        CustomFixedUpdate();
    }

    private void LateUpdate()
    {
        CustomLateUpdate();
    }

    protected virtual void CustomUpdate()
    {
        Operator.RunUpdate();
    }

    protected virtual void CustomFixedUpdate()
    {
        Operator.RunFixedUpdate();
    }

    protected virtual void CustomLateUpdate()
    {
        Operator.RunLateUpdate();
    }
}
