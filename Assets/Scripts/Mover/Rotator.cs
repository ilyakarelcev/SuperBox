using Cephei;
using UnityEngine;

public class Rotator : MoverBase
{
    [SerializeField] private float _speedRotate = 180;
    [SerializeField] private Transform _transform;

    private void Update()
    {
        if (IsMove == false) return;

        _transform.rotation = transform.forward.ZeroY().GetRotation();

        Vector3 toTarget = Target.ZeroY() - _transform.position.ZeroY();
        _transform.RotateTowards(toTarget.GetRotation(), _speedRotate * Time.deltaTime);

        if (Vector3.Angle(toTarget.ZeroY(), _transform.forward.ZeroY()) < 0.5f)
        {
            InvokeComeToTargetEvent();
        }
    }
}