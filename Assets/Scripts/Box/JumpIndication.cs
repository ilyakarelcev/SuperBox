using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpIndication : MonoBehaviour
{
    [SerializeField] private float _maxLength = 7;
    [SerializeField] private AnimationCurve _lineLenghtCurve;
    [Space]
    [SerializeField] private LineRenderer _maxJumpLine;
    [SerializeField] private LineRenderer _curentJumpLine;
    [SerializeField] private Transform _targetCircle;

    private void Awake() {
        _targetCircle.transform.parent = null;
    }

    public void UpdateLines(Vector3 origin, Vector3 direction, float inputMultiply, float charge) 
    {
        float _maxLineLenght = _lineLenghtCurve.Evaluate(inputMultiply) * _maxLength;
        _maxJumpLine.SetPositions(new Vector3[] { origin, origin + direction * _maxLineLenght });

        float _curentJumpLenght = _lineLenghtCurve.Evaluate(Mathf.Min(inputMultiply, charge)) * _maxLength;
        Vector3 targetPosition = origin + direction * _curentJumpLenght;
        _curentJumpLine.SetPositions(new Vector3[] { origin + Vector3.up * 0.05f, targetPosition });
        _targetCircle.position = targetPosition;
    }

    public void Show()
    {
        _maxJumpLine.enabled = true;
        _curentJumpLine.enabled = true;
        _targetCircle.gameObject.SetActive(true);
    }

    public void Hide()
    {
        _maxJumpLine.enabled = false;
        _curentJumpLine.enabled = false;
        _targetCircle.gameObject.SetActive(false);
    }
}
