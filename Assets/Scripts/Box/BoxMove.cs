using System.Collections;
using UnityEngine;
using Cephei;

public class BoxMove : MonoBehaviour, IPersonComponent
{
    [SerializeField] private float _jumpVelocity = 8;
    [SerializeField] private float _jumpTreshHold = 1;
    [SerializeField, Range(0, 1)] private float _recoverPercentFromTime = 0.5f;
    [Space]
    [SerializeField] private AnimationCurve _jumpMultiplyCurve;
    [Space]
    [SerializeField] private Collider _boxCollider;
    [SerializeField] private Rigidbody _rb;

    public bool BoxIsJump => _rb.velocity.sqrMagnitude > _jumpTreshHold.Sqr();

    private Vector3 _startBoxColliderScale;
    private Coroutine _recoveryScaleRoutine;

    [Space]
    [SerializeField] private BoxMoveDebug _debug;

    public IPerson Person { get; private set; }

    public void Init(IPerson person)
    {
        Person = person;

        _startBoxColliderScale = _boxCollider.transform.localScale;

        person.GetPersonComponentIs<IJumpInfoConteiner>().JumpEvent += Jump;
    }

    private void Update()
    {
        _debug.BoxIsJump = BoxIsJump;
    }

    public void Jump(IJumpInfoConteiner jumpInfo)
    {
        float force = jumpInfo.MaxVelosity * _jumpMultiplyCurve.Evaluate(jumpInfo.Multiply);
        _rb.AddForce(jumpInfo.Direction * force, ForceMode.VelocityChange);

        if (_recoveryScaleRoutine != null)
            StopCoroutine(_recoveryScaleRoutine);
        _recoveryScaleRoutine = StartCoroutine(RecoveryBoxColliderScale(jumpInfo.Duration * jumpInfo.Multiply));
    }

    // Idea: create jumpTimeMultiplyCurve to receive more controle

    private IEnumerator RecoveryBoxColliderScale(float jumpTime)
    {
        float jumpDuration = jumpTime * _recoverPercentFromTime;
        for (float t = 0; t <= 1; t += Time.deltaTime / jumpDuration)
        {
            _boxCollider.transform.localScale = Vector3.Lerp(Vector3.zero, _startBoxColliderScale, t);

            yield return null;
        }
        _boxCollider.transform.localScale = Vector3.one;
        _recoveryScaleRoutine = null;
    }

    [System.Serializable]
    private class BoxMoveDebug
    {
        public bool BoxIsJump;
    }
}
