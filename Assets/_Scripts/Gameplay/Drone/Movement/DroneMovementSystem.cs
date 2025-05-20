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
    [SerializeField] private DronePropertiesHolderSO _dronePropertiesHolderSO;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _cameraTransform;

    [Header("Motors")]
    [SerializeField] private float _motorsPowerClamp;
    [SerializeField] private float _maxThrottleValueMultiplier;
    [SerializeField] private float _motorsAccelerationMultiplier;

    [Header("Camera")]
    [SerializeField] private float _cameraInterpolationMultiplier;

    //RC Rate, Super Rate, RC Expo ... 

    public Vector3 Velocity => _rigidbody.velocity;

    private float _droneSpeed;
    private float _yawMotorPower;
    private float _throttleMotorPower;
    private Vector2 _pitchAndRollMotorPower;
    private SignalBus _signalBus;
    private PlayerSettingsSO.DroneFlightModeType _droneFlightMode;
    private IDroneMoveable _droneMoveable;
    private IDroneFlightModeMovementAdjuster _droneFlightModeMovementAdjuster;

    [Inject]
    private void Construct(IDroneMoveable droneMoveable, SignalBus signalBus)
    {
        _droneMoveable = droneMoveable;
        _signalBus = signalBus;

        _signalBus.Subscribe<PlayerSettingsChangedSignal>(ChangeDroneFlightMode);
    }

    private void ChangeDroneFlightMode(PlayerSettingsChangedSignal signal)
    {
        _droneFlightMode = signal.PlayerSettingsSO.DroneFlightMode;
    }

    public float GetThrottleMotorPowerNormalized()
    {
        float value = _throttleMotorPower / _motorsPowerClamp;
        return value;
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<PlayerSettingsChangedSignal>(ChangeDroneFlightMode);
    }

    private void Awake()
    {
        _droneSpeed = _dronePropertiesHolderSO.Speed;
        _droneFlightModeMovementAdjuster = new DroneHorizonMovementAdjuster();
        _droneFlightModeMovementAdjuster.ActionAngleThreshold = 30f;
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
        return normalizedMotorPower * _motorsPowerClamp;
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
        //Debug.Log(pitchRollDelta + " " + transform.eulerAngles);
        Quaternion yawDelta = Quaternion.Euler(
            0,
            _yawMotorPower * _droneSpeed * Time.fixedDeltaTime,
            0
        );

        Quaternion newRotation = _rigidbody.rotation * pitchRollDelta * yawDelta;
        _rigidbody.MoveRotation(newRotation);
    }
}


