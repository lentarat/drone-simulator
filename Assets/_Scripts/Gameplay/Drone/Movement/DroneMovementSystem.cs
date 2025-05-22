using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

public class DroneMovementSystem : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private DronePropertiesHolderSO _dronePropertiesHolderSO;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _cameraTransform;

    [Header("Motors")]
    [SerializeField] private float _motorsPowerClamp;
    [SerializeField] private float _maxThrottleValueMultiplier;
    [SerializeField] private float _motorsAccelerationMultiplier;

    [Header("Camera")]
    [SerializeField] private float _cameraInterpolationMultiplier;

    public Vector3 Velocity => _rigidbody.velocity;

    private float _droneSpeed;
    private float _throttleMotorPower;
    private float _yawMotorPower;
    private Vector2 _pitchAndRollMotorPower;
    private DroneFlightModeMovementAdjuster _droneFlightModeMovementAdjuster = new DroneAcroMovementAdjuster();
    private DronePlayerSettingsChangesHandler _dronePlayerSettingsChangesHandler;
    private IDroneMoveable _droneMoveable;

    [Inject]
    private void Construct(IDroneMoveable droneMoveable, DronePlayerSettingsChangesHandler dronePlayerSettingsChangesHandler)
    {
        _droneMoveable = droneMoveable;
        _dronePlayerSettingsChangesHandler = dronePlayerSettingsChangesHandler;

        _dronePlayerSettingsChangesHandler.OnDronePlayerSettingsChanged += ChangeDroneFlightModeMovementAdjuster;
    }

    private void ChangeDroneFlightModeMovementAdjuster(DroneFlightModeMovementAdjuster droneFlightModeMovementAdjuster)
    {
        _droneFlightModeMovementAdjuster = droneFlightModeMovementAdjuster;
    }

    private void OnDestroy()
    {
        _dronePlayerSettingsChangesHandler.OnDronePlayerSettingsChanged -= ChangeDroneFlightModeMovementAdjuster;
    }

    public float GetThrottleMotorPowerNormalized()
    {
        float value = _throttleMotorPower / _motorsPowerClamp;
        return value;
    }

    public float GetYawMotorPowerNormalized()
    { 
        float value = Mathf.Abs(_yawMotorPower / _motorsPowerClamp);
        return value;
    }

    public float GetPitchAndRollMotorPowerNormalized()
    {
        float pitchValue = Mathf.Abs(_pitchAndRollMotorPower.y / _motorsPowerClamp);
        float rollValue = Mathf.Abs(_pitchAndRollMotorPower.x / _motorsPowerClamp);
        float value = Mathf.Max(pitchValue, rollValue);
        return value;
    }

    private void Awake()
    {
        _droneSpeed = _dronePropertiesHolderSO.Speed;
    }

    private void Update()
    {
        UpdateMotorsPowers();
        UpdateCamera();
    }

    private void UpdateMotorsPowers()
    {
        Vector2 pitchAndRollInputVector = _droneMoveable.GetPitchAndRollInputValue;
        Vector2 adjustedPitchAndRollInputVector = _droneFlightModeMovementAdjuster.GetAdjustedPitchAndRollInputVector(pitchAndRollInputVector, _rigidbody.rotation);

        _pitchAndRollMotorPower.x = GetAdjustedMotorPowerAccordingInputValue(adjustedPitchAndRollInputVector.x, _pitchAndRollMotorPower.x);
        _pitchAndRollMotorPower.y = GetAdjustedMotorPowerAccordingInputValue(adjustedPitchAndRollInputVector.y, _pitchAndRollMotorPower.y);

        float yawInput = _droneMoveable.GetYawInputValue;
        _yawMotorPower = GetAdjustedMotorPowerAccordingInputValue(yawInput, _yawMotorPower);

        float throttleInput = _droneMoveable.GetThrottleInputValue;
        _throttleMotorPower = GetAdjustedMotorPowerAccordingInputValue(throttleInput, _throttleMotorPower);
    }

    private float GetAdjustedMotorPowerAccordingInputValue(float inputValue, float currentMotorPowerValue)
    {
        float normalizedMotorPower = currentMotorPowerValue / _motorsPowerClamp;
        float t = _motorsAccelerationMultiplier * Time.deltaTime;
        normalizedMotorPower = Mathf.Lerp(normalizedMotorPower, inputValue, t);
        float adjustedMotorPower = normalizedMotorPower * _motorsPowerClamp;
        return adjustedMotorPower;
    }

    private void UpdateCamera()
    {
        _cameraTransform.rotation = Quaternion.Lerp(_cameraTransform.rotation, _rigidbody.rotation, _cameraInterpolationMultiplier * Time.deltaTime);
        _cameraTransform.position = transform.position;
    }

    private void FixedUpdate()
    {
        if (_throttleMotorPower > 0.1f)
        { 
            Vector3 upForceVector = transform.up * _throttleMotorPower * _maxThrottleValueMultiplier * _droneSpeed;
            _rigidbody.AddForce(upForceVector);
        }

        UpdateRotation();
    }

    private void UpdateRotation()
    {
        Quaternion pitchRollDelta = Quaternion.Euler(
            -_pitchAndRollMotorPower.x * _droneSpeed * Time.fixedDeltaTime,
            0,
            -_pitchAndRollMotorPower.y * _droneSpeed * Time.fixedDeltaTime
        );
        Quaternion yawDelta = Quaternion.Euler(
            0,
            _yawMotorPower * _droneSpeed * Time.fixedDeltaTime,
            0
        );

        Quaternion newRotation = _rigidbody.rotation * pitchRollDelta * yawDelta;
        _rigidbody.MoveRotation(newRotation);
    }
}


