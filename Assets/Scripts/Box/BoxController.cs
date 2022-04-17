using Cephei;
using UnityEngine;

public class BoxController : MonoBehaviour, IPersonComponent
{
    [SerializeField] private float _jumpVelosity = 22;
    [Space]
    [SerializeField] private float _jumpDuration = 2;
    [SerializeField] private float _timeChargeRecovery = 2;
    [SerializeField] private float _maxJumpPrice = 0.6f;
    [Space]
    [SerializeField] private Joystick _joystick;
    [SerializeField] private JumpIndication _jumpIndication;
    [SerializeField] private ChargeIndicationOnJoystic _chargeIndication;
    [SerializeField] private CameraMove _cameraMove;
    [SerializeField] private Rigidbody _rigidbody;

    public float Charge { get; private set; } = 1;
    public Jump CurentJump { get; private set; }

    public float VelosityPercent => _rigidbody.velocity.magnitude / _jumpVelosity;

    [SerializeField] private PlayerBoxDebug _debug;

    private IJumpInfoTaker _jumpInfoTaker;
    
    public IPerson Person { get; private set; }

    public void Init(IPerson person)
    {
        Person = person;

        _joystick.OnUpEvent += OnJoystickUp;

        _joystick.OnDownEvent += (x) => _jumpIndication.Show();
        _joystick.OnPressedEvent += (x) =>
        {
            float inputMagintude = _joystick.Value.magnitude;
            _jumpIndication.UpdateLines(transform.position, GetDirectionFromInput(_joystick.Value),
                inputMagintude, GetJumpMultiplyByInput(inputMagintude));
        };
        _joystick.OnUpEvent += (x) => _jumpIndication.Hide();

        _joystick.OnPressedEvent += HandleChargeIndication;
        _joystick.OnUpEvent += (i) => _chargeIndication.EndUseReverse();

        _jumpInfoTaker = person.GetPersonComponentIs<IJumpInfoTaker>();
    }

    private void Update()
    {
        if (Charge < 1)
            Charge += Time.deltaTime / _timeChargeRecovery;
        else
            Charge = 1;

        _chargeIndication.SetCharge(Charge); // ChargePercent;

        _debug.UpdateInfo(Charge);
    }

    private void OnJoystickUp(Vector2 input)
    {
        if (_joystick.Value == Vector2.zero) return;

        float inputMagnitude = _joystick.Value.magnitude;
        float jumpMultiply = GetJumpMultiplyByInput(inputMagnitude);

        Vector3 direction = GetDirectionFromInput(_joystick.Value);
        _jumpInfoTaker.TakeNewJumpInfo(direction, jumpMultiply, Charge, inputMagnitude);


        //_cameraMove.TargetDirection = direction;
        CurentJump = new Jump(direction, jumpMultiply, _jumpDuration * jumpMultiply);//
        _debug.UpdateJumpInfo(CurentJump);

        Charge -= inputMagnitude * _maxJumpPrice; 
        Charge = Mathf.Max(Charge, 0);
    }

    private Vector3 GetDirectionFromInput(Vector2 input)
    {
        //return _cameraMove.DirectionToLocal(inputDirection);

        return new Vector3(-1 * _joystick.Value.x, 0, -1 * _joystick.Value.y).normalized;
    }

    private float GetJumpMultiplyByInput(float inputMagnitude)
    {
        float inputCoificent = inputMagnitude * _maxJumpPrice;
        return Mathf.Min(inputCoificent, Charge) / _maxJumpPrice;
    }

    private void HandleChargeIndication(Vector2 input)
    {
        float inputMagnitude = _joystick.Value.magnitude * _maxJumpPrice; 
        float jumpMultiply = Mathf.Min(inputMagnitude, Charge); 

        _chargeIndication.SetReverse(jumpMultiply);
    }

    [System.Serializable]
    private class PlayerBoxDebug
    {
        public float ChargeView;
        public float TimeSinceLastJump;

        [Header("Jump values")]
        public Vector3 Direction;
        public float DirectionMagnitude;
        public float ChargeOnJump;
        public float EvaluateMultiply;

        public void UpdateInfo(float charge)
        {
            ChargeView = charge;
            TimeSinceLastJump += Time.deltaTime;
        }

        public void UpdateJumpInfo(Jump jump)
        {
            Direction = jump.Direction;
            DirectionMagnitude = jump.Multiply;
            ChargeOnJump = ChargeView;

            TimeSinceLastJump = 0;
        }
    }
}
