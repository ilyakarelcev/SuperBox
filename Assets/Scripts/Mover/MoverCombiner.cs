using System;
using UnityEngine;

public class MoverCombiner : IMover
{
    public MoverCombiner(params IMover[] movers)
    {
        if (movers.Length == 0)
            Debug.LogError("Zero element in array");

        Movers = movers;
    }

    public Vector3 Target => Movers[0].Target;

    public bool IsMove => Movers[0].IsMove;

    public event Action ComeToTarget;

    public IMover[] Movers { get; private set; }

    public void SetTarget(Vector3 position)
    {
        ByPassMovers(x => x.SetTarget(position));
    }

    public void SetTarget(Transform targetTransform)
    {        
        ByPassMovers(x => x.SetTarget(targetTransform));
    }

    public void StartMove()
    {
        ByPassMovers(x => x.StartMove());
    }

    public void StopMove()
    {
        ByPassMovers(x => x.StopMove());
    }

    private void ByPassMovers(Action<IMover> action)
    {
        foreach (var mover in Movers)
        {
            action.Invoke(mover);
        }
    }
}