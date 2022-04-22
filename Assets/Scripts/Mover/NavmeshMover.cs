using Cephei;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshMover : MoverBase
{
    [SerializeField] private NavMeshAgent _agent;

    [SerializeField, HideInInspector]  private float _agentSpeed;

    private void OnValidate()
    {
        _agentSpeed = _agent.speed;
    }

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
        //_agent.enabled = false;
                
        _agent.speed = 0;
        _agent.velocity = Vector3.zero;
    }

    public override void StartMove()
    {
        base.StartMove();
        //_agent.enabled = true;

        _agent.speed = _agentSpeed;
    }
    
    public void SetEnabledForNavmesh(bool enabled)
    {
        _agent.enabled = enabled;
    }
}
