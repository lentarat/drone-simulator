using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class DroneMovementSystem : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private DronePropertiesHolderSO _dronePropertiesHolderSO;

    [Header("Motors")]
    [SerializeField] private float _motorsPowerClamp;
    [SerializeField] private float _throttleMultiplier;
    [SerializeField] private float _motorsAccelerationMultiplier;
    //[SerializeField] private float _pitchAndRollAccelerationMultiplier;

    //RC Rate, Super Rate, RC Expo ... 

    public Vector3 Velocity => _rigidbody.velocity;

    private float _droneSpeed;
    private float _yawMotorPower;
    private float _throttleMotorPower;
    //private float _compensateFixedDeltaTimeMultiplier = 200f;
    private Vector2 _pitchAndRollMotorPower;
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
    }

    private void FixedUpdate()
    {
        HandlePitchAndRoll();
        HandleYaw();
        HandleThrottle();
    }

    private void HandlePitchAndRoll()
    {
        Vector2 inputVector = _droneMoveable.GetPitchAndRollInputValue;
        if (inputVector == Vector2.zero)
        {
            Vector2 minPower = Vector2.one * 0.01f;
            if (Mathf.Abs(_pitchAndRollMotorPower.x) < minPower.x && Mathf.Abs(_pitchAndRollMotorPower.y) < minPower.y)
                return;
        }

        _pitchAndRollMotorPower.x = GetAdjustedMotorPowerAccordingInputValue(inputVector.x, _pitchAndRollMotorPower.x/*, _pitchAndRollAccelerationMultiplier*/);
        _pitchAndRollMotorPower.y = GetAdjustedMotorPowerAccordingInputValue(inputVector.y, _pitchAndRollMotorPower.y/*, _pitchAndRollAccelerationMultiplier*/);

        if (_pitchAndRollMotorPower != Vector2.zero)
        {
            transform.Rotate(new Vector3(-_pitchAndRollMotorPower.x, 0f, -_pitchAndRollMotorPower.y) * _droneSpeed, Space.Self);
        }
    }

    private float GetAdjustedMotorPowerAccordingInputValue(float inputValue, float currentMotorPowerValue/*, float additionalAccelerationMultiplier = 1f*/)
    {
        float normalizedMotorPower = currentMotorPowerValue / _motorsPowerClamp;
        float t = _motorsAccelerationMultiplier /** additionalAccelerationMultiplier */* Time.fixedDeltaTime;
        normalizedMotorPower = Mathf.Lerp(normalizedMotorPower, inputValue, t);
        return normalizedMotorPower * _motorsPowerClamp;
    }

    private void HandleYaw()
    {
        float inputValue = _droneMoveable.GetYawInputValue;
        if (inputValue == 0f)
        {
            if (Mathf.Abs(_yawMotorPower) < 0.01f)
                return;
        }

        _yawMotorPower = GetAdjustedMotorPowerAccordingInputValue(inputValue, _yawMotorPower);
        if (_yawMotorPower != 0f)
        {
            transform.Rotate(new Vector3(0f, _yawMotorPower * _droneSpeed, 0f), Space.Self);
        }
    }

    private void HandleThrottle()
    {
        float inputValue = Mathf.Clamp01(_droneMoveable.GetThrottleInputValue);
        if (inputValue == 0f)
        {
            if (_throttleMotorPower < 0.01f)
            {
                _throttleMotorPower = 0f;
                return;
            }
        }

        _throttleMotorPower = GetAdjustedMotorPowerAccordingInputValue(inputValue, _throttleMotorPower);
        if (_throttleMotorPower != 0f)
        {
            _rigidbody.AddForce(transform.up * _throttleMultiplier * _throttleMotorPower * _droneSpeed);
        }
    }
}
