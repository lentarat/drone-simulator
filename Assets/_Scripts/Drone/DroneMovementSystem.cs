using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovementSystem : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private DronePropertiesHolderSO _dronePropertiesHolderSO;
    [SerializeField] private float _throttleMultiplicator;
    [SerializeField] private float _accelerationMultiplier;

    private float _droneSpeed;
    private float _yawMotorPower;
    private float _throttleMotorPower;
    private Vector2 _pitchAndRollMotorPower;
    private DroneMovementInputActionsReader _droneMovementInputActionsReader;

    private void Awake()
    {
        _droneMovementInputActionsReader = new();
        _droneSpeed = _dronePropertiesHolderSO.Speed;
    }

    private void OnEnable()
    {
        _droneMovementInputActionsReader.Enable();
    }

    private void OnDisable()
    {
        _droneMovementInputActionsReader.Disable();
    }

    private void FixedUpdate()
    {
        HandlePitchAndRoll();
        HandleYaw();
        HandleThrottle();
    }

    private void HandlePitchAndRoll()
    {
        Vector2 pitchAndRollDelta = _droneMovementInputActionsReader.GetPitchAndRollInputValue;
        if (pitchAndRollDelta == Vector2.zero)
        {
            if (_pitchAndRollMotorPower == Vector2.zero)
            {
                return;
            }
            else
            {
                SmootlyPowerDownPitchAndRoll();
            }
        }
        else
        {
            _pitchAndRollMotorPower += pitchAndRollDelta * _droneSpeed * _accelerationMultiplier * Time.fixedDeltaTime;
            _pitchAndRollMotorPower.x = Mathf.Sign(_pitchAndRollMotorPower.x) * Mathf.Clamp01(Mathf.Abs(_pitchAndRollMotorPower.x));
            _pitchAndRollMotorPower.y = Mathf.Sign(_pitchAndRollMotorPower.y) * Mathf.Clamp01(Mathf.Abs(_pitchAndRollMotorPower.y));
        }

        if (_pitchAndRollMotorPower != Vector2.zero)
        {
            transform.Rotate(new Vector3(_pitchAndRollMotorPower.y, 0f, -_pitchAndRollMotorPower.x) * _droneSpeed, Space.Self);
        }
    }

    private void SmootlyPowerDownPitchAndRoll()
    {
        if (_pitchAndRollMotorPower.y > 0f)
        {
            _pitchAndRollMotorPower.y -= _droneSpeed * _accelerationMultiplier * Time.fixedDeltaTime;
            if (_pitchAndRollMotorPower.y < 0f)
                _pitchAndRollMotorPower.y = 0f;
        }
        else
        {
            _pitchAndRollMotorPower.y += _droneSpeed * _accelerationMultiplier * Time.fixedDeltaTime;
            if (_pitchAndRollMotorPower.y > 0f)
                _pitchAndRollMotorPower.y = 0f;
        }
        if (_pitchAndRollMotorPower.x > 0f)
        {
            _pitchAndRollMotorPower.x -= _droneSpeed * _accelerationMultiplier * Time.fixedDeltaTime;
            if (_pitchAndRollMotorPower.x < 0f)
                _pitchAndRollMotorPower.x = 0f;
        }
        else
        {
            _pitchAndRollMotorPower.x += _droneSpeed * _accelerationMultiplier * Time.fixedDeltaTime;
            if (_pitchAndRollMotorPower.x > 0f)
                _pitchAndRollMotorPower.x = 0f;
        }
    }

    private void HandleYaw()
    {
        float yawDelta = _droneMovementInputActionsReader.GetYawInputValue;
        if (yawDelta == 0f)
        {
            if (_yawMotorPower == 0f)
            {
                return;
            }
            else
            {
                SmootlyPowerDownYaw();
            }
        }
        else
        {
            _yawMotorPower += yawDelta * _droneSpeed * _accelerationMultiplier * Time.fixedDeltaTime;
            _yawMotorPower = Mathf.Sign(_yawMotorPower) * Mathf.Clamp01(Mathf.Abs(_yawMotorPower));
        }

        if (_yawMotorPower != 0f)
        {
            transform.Rotate(new Vector3(0f, _yawMotorPower * _droneSpeed, 0f), Space.Self);
        }
    }

    private void SmootlyPowerDownYaw()
    {
        if (_yawMotorPower > 0f)
        {
            _yawMotorPower -= _droneSpeed * _accelerationMultiplier * Time.fixedDeltaTime;
            if (_yawMotorPower < 0f)
                _yawMotorPower = 0f;
        }
        else
        {
            _yawMotorPower += _droneSpeed * _accelerationMultiplier * Time.fixedDeltaTime;
            if (_yawMotorPower > 0f)
                _yawMotorPower = 0f;
        }
    }

    private void HandleThrottle()
    {
        float throttleDelta = _droneMovementInputActionsReader.GetThrottleInputValue;
        if (throttleDelta == 0f)
        {
            if (_throttleMotorPower == 0f)
            {
                return;
            }
            else
            {
                SmootlyPowerDownThrottle();
            }
        }
        else
        {
            _throttleMotorPower += throttleDelta * _droneSpeed * _accelerationMultiplier * Time.fixedDeltaTime;
            _throttleMotorPower = Mathf.Sign(_throttleMotorPower) * Mathf.Clamp01(Mathf.Abs(_throttleMotorPower));
        }

        if (_throttleMotorPower != 0f)
        {
            _rigidbody.AddForce(transform.up * _throttleMultiplicator * _throttleMotorPower * _droneSpeed);
        }
    }

    private void SmootlyPowerDownThrottle()
    {
        if (_throttleMotorPower > 0f)
        {
            _throttleMotorPower -= _droneSpeed * _accelerationMultiplier * Time.fixedDeltaTime;
            if (_throttleMotorPower < 0f)
                _throttleMotorPower = 0f;
        }
        else
        {
            _throttleMotorPower += _droneSpeed * _accelerationMultiplier * Time.fixedDeltaTime;
            if (_throttleMotorPower > 0f)
                _throttleMotorPower = 0f;
        }
    }
}
