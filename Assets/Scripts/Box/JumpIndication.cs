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

    public void UpdateLines(Vector3 origin, Vector3 direction, float inputMultiply, float charge) 
    {
        float _maxLineLenght = _lineLenghtCurve.Evaluate(inputMultiply) * _maxLength;
        _maxJumpLine.SetPositions(new Vector3[] { origin, origin + direction * _maxLineLenght });

        float _curentJumpLenght = _lineLenghtCurve.Evaluate(Mathf.Min(inputMultiply, charge)) * _maxLength;
        _curentJumpLine.SetPositions(new Vector3[] { origin + Vector3.up * 0.05f, origin + direction * _curentJumpLenght });
    }

    public void Show()
    {
        _maxJumpLine.enabled = true;
        _curentJumpLine.enabled = true;
    }

    public void Hide()
    {
        _maxJumpLine.enabled = false;
        _curentJumpLine.enabled = false;
    }
}
