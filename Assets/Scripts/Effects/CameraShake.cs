using Cephei;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float _duration = 1;
    [SerializeField, Range(0, 1)] private float _randomFactor = 0;
    [Space]
    [SerializeField] private AnimationCurve _magnitudeOnDurationCurve;
    [SerializeField] private AnimationCurve _speedOnDurationCurve;

    [Header("Position parametrs")]
    [SerializeField] private float _magnitude = 1;
    [SerializeField] private float _speedMove = 10;
    [SerializeField] private float _minDotDifference = 0;

    [Header("Position parametrs")]
    [SerializeField] private float _angle = 20;
    [SerializeField] private float _speedRotate = 150;
    [Space]
    [SerializeField] private Transform _cameraTransform;

    private Vector3 _startLocalPosition;
    private Quaternion _startLocalRotation;

    [Space]
    public bool Test;
    public float TestMultiply;

    private void Start()
    {
        _startLocalPosition = _cameraTransform.localPosition;
        _startLocalRotation = _cameraTransform.localRotation;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Test)
        {            
            Shake(TestMultiply);
            Test = false;
        }
    }

    public void Shake(float multiply)
    {
        StopAllCoroutines();

        StartCoroutine(PositionShake(multiply));
        StartCoroutine(RotationShake(multiply));
    }

    private IEnumerator PositionShake(float multiply)
    {
        float timer = 0;

        Vector3 targetPosition = _cameraTransform.localPosition;
        float duration = _duration;
        float magnitude = _magnitude * multiply;
        float speed = _speedMove * multiply;

        Vector3 curentDirection = Random.insideUnitSphere.ZeroY().normalized;

        while (timer < duration)
        {
            float t = timer / duration;

            if (Vector3.Distance(targetPosition, _cameraTransform.localPosition) < 0.05f)
            {
                float curentMagnitude = magnitude * _magnitudeOnDurationCurve.Evaluate(t);
                float magnitudeWithRandomFactor = GetValueWithRandomFactor(curentMagnitude);
                curentDirection = GetNewOffset(curentDirection);

                targetPosition = _startLocalPosition + (curentDirection * magnitudeWithRandomFactor);
            }

            float speedOnThisFrame = speed * _speedOnDurationCurve.Evaluate(t) * Time.deltaTime;
            _cameraTransform.localPosition = Vector3.MoveTowards(_cameraTransform.localPosition, targetPosition, speedOnThisFrame);

            timer += Time.deltaTime;
            yield return null;
        }
    }

    private Vector3 GetNewOffset(Vector3 oldOffset)
    {
        Vector3 randomDirection = Random.insideUnitSphere.ZeroY().normalized;

        while (Vector3.Dot(oldOffset, randomDirection) > _minDotDifference)
            randomDirection = Random.insideUnitSphere.ZeroY().normalized;

        return randomDirection;
    }

    private IEnumerator RotationShake(float multiply)
    {
        float timer = 0;

        Quaternion targetRotation = _cameraTransform.localRotation;
        float duration = _duration;
        float angle = _angle * multiply;
        float speed = _speedRotate * multiply;

        float sign = Cephei.CustomRandom.Sign();

        while (timer < duration)
        {
            float t = timer / duration;

            if (Quaternion.Angle(targetRotation, _cameraTransform.localRotation) < 0.5f)
            {
                float curentAngle = angle * _magnitudeOnDurationCurve.Evaluate(t);
                float angleWithRandomFactor = GetValueWithRandomFactor(curentAngle);
                sign *= -1;

                targetRotation = _startLocalRotation * Quaternion.AngleAxis(angleWithRandomFactor * sign, Vector3.forward);
            }

            float speedOnThisFrame = speed * _speedOnDurationCurve.Evaluate(t) * Time.deltaTime;
            _cameraTransform.localRotation = Quaternion.RotateTowards(_cameraTransform.localRotation, targetRotation, speedOnThisFrame);

            timer += Time.deltaTime;
            yield return null;
        }
    }

    private float GetValueWithRandomFactor(float value)
    {
        return value + value * Random.Range(0, _randomFactor) * Cephei.CustomRandom.Sign();
    }
}