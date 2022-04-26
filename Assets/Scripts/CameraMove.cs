using Cephei;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private float _speedMove = 5;
    [SerializeField] private float _speedRotate = 5;
    [Space]
    [SerializeField] private Transform _cameraCentr;
    [SerializeField] private Transform _target;

    private Vector3 _targetDirection = Vector3.forward;

    public Vector3 TargetDirection { get => _targetDirection; set => _targetDirection = value; }
    public Vector3 DirectionToLocal(Vector3 worldDirection) => _cameraCentr.CustomInverseTransformVector(worldDirection);

    public bool TestDirection;
    public Vector3 direction;

    public void LateUpdate()
    {
        _cameraCentr.position = Vector3.Lerp(_cameraCentr.position, _target.position, _speedMove * Time.deltaTime);
        _cameraCentr.rotation = Quaternion.Slerp(_cameraCentr.rotation, TargetDirection.GetRotation(), _speedRotate * Time.deltaTime);
    }

    public void SetEnabled(bool enabled)
    {
        this.enabled = enabled;
    }
}