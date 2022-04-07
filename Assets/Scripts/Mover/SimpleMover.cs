using Cephei;
using UnityEngine;

public class SimpleMover : MoverBase
{
    [SerializeField] private float _speedMove = 5;
    [SerializeField] private Transform _transform;    

    private void Update()
    {
        if (IsMove == false) return;

        _transform.MoveForward(_speedMove * Time.deltaTime);

        Vector3 toTarget = Target.ZeroY() - _transform.position.ZeroY();
        if (toTarget.magnitude < 0.1f) InvokeComeToTargetEvent();
    }
}