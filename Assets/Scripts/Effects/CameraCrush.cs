using System.Collections;
using UnityEngine;

public class CameraCrush : MonoBehaviour
{
    [Header("Position")]
    [SerializeField] private float _magnitude = 1;
    [SerializeField] private float _magnitudeToNormalize;
    [SerializeField] private float _decraceMove = 0.2f;
    [SerializeField] private float _speedMove = 5;
    [SerializeField] private float _timeMove = 0.5f;
    [Header("Rotation")]
    [SerializeField] private float _maxAngle = 1;
    [SerializeField] private float _angleToNormalize = 3;
    [SerializeField] private float _decraceRotation = 0.2f;
    [SerializeField] private float _speedRotate = 5;
    [SerializeField] private float _timeRotate = 0.5f;
    [Space]
    [SerializeField] private float _speedNormalize = 0.5f;
    [SerializeField] private float _pogreshnost = 0.5f;
    [Space]
    [SerializeField] private Transform _cameraTransform;

    public bool IsCrush { get => _isPositionCrush && _isRotationCrush; }
    private bool _isPositionCrush = false;
    private bool _isRotationCrush = false;

    private Vector3 _beforCrushLocalPosition;
    private Quaternion _beforCrushLocalRotation;

    [Space]
    public bool Test;
    public float Multiply = 1;

    private void Start()
    {
        _beforCrushLocalPosition = _cameraTransform.localPosition;
        _beforCrushLocalRotation = _cameraTransform.localRotation;
    }

    public void Update()
    {
        if (Test)
            AddCrush(Multiply);
        Test = false;
    }

    public void AddCrush(float multiply)
    {
        if (IsCrush) StopAllCoroutines();        

        StartCoroutine(CrushCameraPosition(_magnitude * multiply, _decraceMove * multiply, _speedMove * multiply, _timeMove * multiply));
        StartCoroutine(CrushCameraRotate(_maxAngle * multiply, _decraceRotation * multiply, _speedRotate * multiply, _timeRotate * multiply));

        _isPositionCrush = true;
        _isRotationCrush = true;

        StartCoroutine(NormolizeCamera());
    }

    private IEnumerator CrushCameraPosition(float magnitude, float decraceMagnitude, float speed, float time)
    {
        Vector3 startPosition = _cameraTransform.localPosition;
        float timer = 0;

        while (true)
        {
            Vector3 newPosition = startPosition + Random.insideUnitSphere.normalized * Random.Range(0.7f, 1) * magnitude;
            //if(Vector3.Distance(newPosition, startPosition) < _pogreshnost)
            //{
            //    _cameraTransform.localPosition = startPosition;
            //    break;
            //}

            while (Vector3.Distance(_cameraTransform.localPosition, newPosition) > speed * Time.deltaTime)
            {
                //print("Corutine is work");
                timer += Time.deltaTime;

                _cameraTransform.localPosition = Vector3.MoveTowards(_cameraTransform.localPosition, newPosition, speed * Time.deltaTime);
                yield return null;
            }
            magnitude *= decraceMagnitude;
            speed *= decraceMagnitude;

            if (magnitude > _magnitudeToNormalize)
                continue;

            while (Vector3.Distance(_cameraTransform.localPosition, _beforCrushLocalPosition) > _pogreshnost)
            {
                _cameraTransform.localPosition = Vector3.Lerp(_cameraTransform.localPosition, _beforCrushLocalPosition, _speedNormalize * Time.deltaTime);
                yield return null;
            }
            _cameraTransform.localPosition = _beforCrushLocalPosition;
            break;
        }
        _isPositionCrush = false;
    }

    private IEnumerator CrushCameraRotate(float maxAngle, float decraceMagnitude, float speed, float time)
    {
        Quaternion startRotation = _cameraTransform.localRotation;
        float timer = 0;

        int sign = Random.Range(-1, 2);
        sign = Mathf.RoundToInt(Mathf.Sign(sign));
        while (true)
        {
            Quaternion newRotation = startRotation * Quaternion.AngleAxis(maxAngle * Random.Range(0.7f, 1) * sign, Vector3.forward);
            while (true)
            {
                //print("Corutine is work");
                timer += Time.deltaTime;
                _cameraTransform.localRotation = Quaternion.RotateTowards(_cameraTransform.localRotation, newRotation, speed * Time.deltaTime);
                if (Quaternion.Angle(_cameraTransform.localRotation, newRotation) < speed * Time.deltaTime)
                {
                    break;
                }
                yield return null;
            }
            maxAngle *= decraceMagnitude;
            speed *= decraceMagnitude;
            sign *= -1;

            if (maxAngle > _angleToNormalize)
                continue;

            while (Quaternion.Angle(_cameraTransform.localRotation, _beforCrushLocalRotation) > _pogreshnost)
            {
                _cameraTransform.localRotation = Quaternion.Slerp(_cameraTransform.localRotation, _beforCrushLocalRotation, _speedNormalize * Time.deltaTime);
                yield return null;
            }
            _cameraTransform.localRotation = _beforCrushLocalRotation;
            break;
        }
        _isRotationCrush = false;
    }

    private IEnumerator NormolizeCamera()
    {
        yield return new WaitUntil(() => IsCrush == false);

        while (Vector3.Distance(_cameraTransform.localPosition, _beforCrushLocalPosition) > _pogreshnost)
        {
            _cameraTransform.localPosition = Vector3.Lerp(_cameraTransform.localPosition, _beforCrushLocalPosition,
                _speedNormalize * Time.deltaTime);
            _cameraTransform.localRotation = Quaternion.Lerp(_cameraTransform.localRotation, _beforCrushLocalRotation, 
                _speedNormalize * Time.deltaTime);
            yield return null;
        }

        _cameraTransform.localPosition = _beforCrushLocalPosition;
        _cameraTransform.localRotation = _beforCrushLocalRotation;
    }
}
