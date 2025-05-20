using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class DroneMovementSystem : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private DronePropertiesHolderSO _dronePropertiesHolderSO;
    [SerializeField] private Transform _cameraTransform;

    [Header("Motors")]
    [SerializeField] private float _motorsPowerClamp;
    [SerializeField] private float _maxThrottleValueMultiplier;
    [SerializeField] private float _motorsAccelerationMultiplier;
    [SerializeField] private float _maxRotationAngularVelocityMultiplier;

    [Header("Camera")]
    [SerializeField] private float _cameraInterpolationMultiplier;

    //RC Rate, Super Rate, RC Expo ... 

    public Vector3 Velocity => _rigidbody.velocity;

    private float _droneSpeed;
    private float _yawMotorPower;
    private float _throttleMotorPower;
    private Vector2 _pitchAndRollMotorPower;
    private Quaternion _targetRotation;
    private IDroneMoveable _droneMoveable;

    [Inject]
    private void Construct(IDroneMoveable droneMoveable)
    {
        _droneMoveable = droneMoveable;
    }

    public float GetThrottleMotorPowerNormalized()
    {
        float value = _throttleMotorPower / _motorsPowerClamp;
        return value;
    }

    private void Awake()
    {
        _droneSpeed = _dronePropertiesHolderSO.Speed;
        _targetRotation = transform.rotation;
    }

    private void Update()
    {
        UpdateMotorsPowers();
        _cameraTransform.rotation = Quaternion.Lerp(_cameraTransform.rotation, _rigidbody.rotation, Time.deltaTime * _maxRotationAngularVelocityMultiplier);
        _cameraTransform.position = transform.position;
    }

    private void UpdateMotorsPowers()
    {
        Vector2 pitchAndRollInputVector = _droneMoveable.GetPitchAndRollInputValue;
        _pitchAndRollMotorPower.x = GetAdjustedMotorPowerAccordingInputValue(pitchAndRollInputVector.x, _pitchAndRollMotorPower.x);
        _pitchAndRollMotorPower.y = GetAdjustedMotorPowerAccordingInputValue(pitchAndRollInputVector.y, _pitchAndRollMotorPower.y);

        float yawInput = _droneMoveable.GetYawInputValue;
        _yawMotorPower = GetAdjustedMotorPowerAccordingInputValue(yawInput, _yawMotorPower);

        float throttleInput = _droneMoveable.GetThrottleInputValue;
        _throttleMotorPower = GetAdjustedMotorPowerAccordingInputValue(throttleInput, _throttleMotorPower );
    }

    private float GetAdjustedMotorPowerAccordingInputValue(float inputValue, float currentMotorPowerValue)
    {
        float normalizedMotorPower = currentMotorPowerValue / _motorsPowerClamp;
        float t = _motorsAccelerationMultiplier * Time.deltaTime;
        normalizedMotorPower = Mathf.Lerp(normalizedMotorPower, inputValue, t);
        return normalizedMotorPower* _motorsPowerClamp;
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
        //_targetRotation *= pitchRollDelta * yawDelta;

        Quaternion newRotation = _rigidbody.rotation * pitchRollDelta * yawDelta;
        _rigidbody.MoveRotation(newRotation);
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

    //private void LateUpdate()
    //{
    //    // 5. Плавно доганяємо transform до фізики
    //    transform.rotation = Quaternion.Lerp(
    //        transform.rotation,
    //        _rigidbody.rotation,
    //        _cameraInterpolationMultiplier * Time.deltaTime
    //    );
    //}

    //private void Update()
    //{
    //    HandlePitchAndRoll();
    //    HandleYaw();
    //    //_rigidbody.rotation = /*_targetRotation;*/Quaternion.Lerp(_rigidbody.rotation, _targetRotation, _cameraInterpolationMultiplier * Time.deltaTime);
    //    transform.Rotate(new Vector3(-_pitchAndRollMotorPower.x, _yawMotorPower, -_pitchAndRollMotorPower.y) * _droneSpeed, Space.Self); /////////////////////////////////// WRONG

    //}

    //private void FixedUpdate()
    //{
    //    HandleThrottle();
    //}

    //private void HandlePitchAndRoll()
    //{
    //    Vector2 inputVector = _droneMoveable.GetPitchAndRollInputValue;
    //    if (inputVector == Vector2.zero)
    //    {
    //        Vector2 minPower = Vector2.one * 0.01f;
    //        if (Mathf.Abs(_pitchAndRollMotorPower.x) < minPower.x && Mathf.Abs(_pitchAndRollMotorPower.y) < minPower.y)
    //            return;
    //    }

    //    _pitchAndRollMotorPower.x = GetAdjustedMotorPowerAccordingInputValue(inputVector.x, _pitchAndRollMotorPower.x/*, _pitchAndRollAccelerationMultiplier*/);
    //    _pitchAndRollMotorPower.y = GetAdjustedMotorPowerAccordingInputValue(inputVector.y, _pitchAndRollMotorPower.y/*, _pitchAndRollAccelerationMultiplier*/);

    //    if (_pitchAndRollMotorPower != Vector2.zero)
    //    {
    //        //Vector3 deltaRotationEuler = new Vector3(-_pitchAndRollMotorPower.x, 0f, -_pitchAndRollMotorPower.y) * _maxRotationAngularVelocityMultiplier * _droneSpeed * Time.fixedDeltaTime;
    //        //Quaternion deltaRotation = Quaternion.Euler(deltaRotationEuler);
    //        //_targetRotation *= deltaRotation;
    //        //_targetRotation.Normalize();
    //        //transform.Rotate(new Vector3(-_pitchAndRollMotorPower.x, 0f, -_pitchAndRollMotorPower.y) * _droneSpeed, Space.Self); /////////////////////////////////// WRONG
    //    }
    //}

    //private float GetAdjustedMotorPowerAccordingInputValue(float inputValue, float currentMotorPowerValue/*, float additionalAccelerationMultiplier = 1f*/)
    //{
    //    float normalizedMotorPower = currentMotorPowerValue / _motorsPowerClamp;
    //    float t = _motorsAccelerationMultiplier /** additionalAccelerationMultiplier */* Time.fixedDeltaTime;
    //    normalizedMotorPower = Mathf.Lerp(normalizedMotorPower, inputValue, t);
    //    return normalizedMotorPower * _motorsPowerClamp;
    //}

    //private void HandleYaw()
    //{
    //    float inputValue = _droneMoveable.GetYawInputValue;
    //    if (inputValue == 0f)
    //    {
    //        if (Mathf.Abs(_yawMotorPower) < 0.01f)
    //            return;
    //    }

    //    _yawMotorPower = GetAdjustedMotorPowerAccordingInputValue(inputValue, _yawMotorPower);
    //    if (_yawMotorPower != 0f)
    //    {
    //        //float yRotation = _yawMotorPower * _maxRotationAngularVelocityMultiplier * _droneSpeed * Time.fixedDeltaTime;
    //        //Quaternion deltaRotation = Quaternion.Euler(0f, yRotation, 0f);
    //        //_targetRotation *= deltaRotation;
    //        //_targetRotation.Normalize();
    //        //transform.Rotate(new Vector3(0f, _yawMotorPower * _droneSpeed, 0f), Space.Self); /////////////////////////////////// WRONG
    //    }
    //}

    //private void HandleThrottle()
    //{
    //    float inputValue = Mathf.Clamp01(_droneMoveable.GetThrottleInputValue);
    //    if (inputValue == 0f)
    //    {
    //        if (_throttleMotorPower < 0.01f)
    //        {
    //            _throttleMotorPower = 0f;
    //            return;
    //        }
    //    }

    //    _throttleMotorPower = GetAdjustedMotorPowerAccordingInputValue(inputValue, _throttleMotorPower);
    //    if (_throttleMotorPower != 0f)
    //    {
    //        _rigidbody.AddForce(transform.up * _throttleMotorPower * _maxThrottleValueMultiplier * _droneSpeed);
    //    }
    //}

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawRay(transform.position, transform.up);
    //}
}


