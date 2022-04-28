using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ChargeIndicationOnJoystic : MonoBehaviour
{
    [SerializeField, Range(0, 360)] private float _maxAngle = 90;
    [SerializeField, Range(0, 2)] private float _sizeMultiply = 1;

    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _chargeImage;
    [SerializeField] private Image _reverseChargeImage;
    [Space]
    [SerializeField] private Joystick _joystick;

    private Vector3 _targetUpDirection;

    private float _curentCharge;
    [SerializeField, HideInInspector] private Vector2 _curentSize;

    private bool _useReverse;

    private void OnValidate()
    {
        _targetUpDirection = Quaternion.AngleAxis(_maxAngle / 2, Vector3.forward) * Vector3.up;
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, _targetUpDirection);

        _backgroundImage.transform.rotation = targetRotation;
        _chargeImage.transform.rotation = targetRotation;

        _backgroundImage.fillAmount = _maxAngle / 360;

        RepositionReversCircle(_maxAngle);

        ChangeSize(_curentSize);        
    }

    private void Start()
    {
        EndUseReverse();        
    }

    public void SetCharge(float charge)
    {
        _curentCharge = charge;

        if (_useReverse == false)
            SetFillAmountForChargeCircle(_curentCharge);
    }

    public void SetReverse(float reverse)
    {
        ShowReverse();

        _reverseChargeImage.fillAmount = _maxAngle / 360 * reverse;
        RepositionReversCircle(_curentCharge * _maxAngle);
        SetFillAmountForChargeCircle(_curentCharge); //- reverse
    }

    public void ShowReverse()
    {
        if (_useReverse) return;
        
        _useReverse = true;
        _reverseChargeImage.enabled = true;
    }

    public void EndUseReverse()
    {
        _useReverse = false;
        _reverseChargeImage.enabled = false;
    }

    private void SetFillAmountForChargeCircle(float charge)
    {
        _chargeImage.fillAmount = _maxAngle / 360 * charge;
    }

    private void RepositionReversCircle(float angle)
    {
        Vector3 targetUpDirectionForReverse = Quaternion.AngleAxis(-1 * angle, Vector3.forward) * _targetUpDirection;
        _reverseChargeImage.transform.rotation = Quaternion.LookRotation(Vector3.forward, targetUpDirectionForReverse);
    }

    public void ChangeSize(Vector2 size)
    {
        _curentSize = size;
        size *= _sizeMultiply; 

        _backgroundImage.rectTransform.sizeDelta = size;
        _chargeImage.rectTransform.sizeDelta = size;
        _reverseChargeImage.rectTransform.sizeDelta = size;
    }
}
