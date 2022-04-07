using Cephei;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshMover : MoverBase
{
    [SerializeField] private NavMeshAgent _agent;

    private void Update()
    {
        if (IsMove == false)
            return;

        _agent.SetDestination(Target);

        Vector3 toTarget = Target.ZeroY() - _agent.transform.position.ZeroY();
        if (toTarget.magnitude < 0.1f) InvokeComeToTargetEvent();
    }

    public override void StopMove()
    {
        base.StopMove();
        _agent.enabled = false;
    }

    public override void StartMove()
    {
        base.StartMove();
        _agent.enabled = true;
    }
}
