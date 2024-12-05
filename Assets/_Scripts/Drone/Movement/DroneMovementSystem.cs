using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DroneMovementSystem : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private DronePropertiesHolderSO _dronePropertiesHolderSO;

    [Header("Motors")]
    [SerializeField] private float _throttleMultiplicator;
    [SerializeField] private float _motorsAccelerationMultiplier;
    [SerializeField] private float _motorsPowerClamp;

    private float _droneSpeed;
    private float _yawMotorPower;
    private float _throttleMotorPower;
    //private float _compensateFixedDeltaTimeMultiplier = 200f;
    private Vector2 _pitchAndRollMotorPower;
    private IDroneMoveable _droneMoveable;

    private void Awake()
    {
        _droneMoveable = new DroneMovementInputActionsReader();
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

        _pitchAndRollMotorPower.x = GetAdjustedMotorPowerAccordingInputValue(inputVector.x, _pitchAndRollMotorPower.x);
        _pitchAndRollMotorPower.y = GetAdjustedMotorPowerAccordingInputValue(inputVector.y, _pitchAndRollMotorPower.y);

        if (_pitchAndRollMotorPower != Vector2.zero)
        {
            transform.Rotate(new Vector3(_pitchAndRollMotorPower.y, 0f, -_pitchAndRollMotorPower.x) * _droneSpeed, Space.Self);
        }
    }

    private float GetAdjustedMotorPowerAccordingInputValue(float inputValue, float currentMotorPowerValue)
    {
        float normalizedMotorPower = currentMotorPowerValue / _motorsPowerClamp;
        float t = _motorsAccelerationMultiplier * Time.fixedDeltaTime;
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
            if(_throttleMotorPower < 0.01f)
                return;
        }

        _throttleMotorPower = GetAdjustedMotorPowerAccordingInputValue(inputValue, _throttleMotorPower);
        if (_throttleMotorPower != 0f)
        {
            _rigidbody.AddForce(transform.up * _throttleMultiplicator * _throttleMotorPower * _droneSpeed);
        }
    }
}
