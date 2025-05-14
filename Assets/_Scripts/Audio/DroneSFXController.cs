using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSFXController : MonoBehaviour
{
    [SerializeField] private DroneMovementSystem _droneMovementSystem;
    [SerializeField] private AudioSource _motorsAudioSource;
    [SerializeField] private float _minMotorsAudioSourcePitch;
    [SerializeField] private float _maxMotorsAudioSourcePitch;
    [SerializeField] private float _minMotorsAudioSourceVolume;
    [SerializeField] private float _maxMotorsAudioSourceVolume;

    private void Update()
    {
        SetPitchAndVolumeAccordingMotorsThrottleValue();
    }

    private void SetPitchAndVolumeAccordingMotorsThrottleValue()
    {
        float throttleMotorPower = _droneMovementSystem.GetThrottleMotorPowerNormalized();
        if (throttleMotorPower > 0)
        {
            float newPitchValue = Mathf.Lerp(_minMotorsAudioSourcePitch, _maxMotorsAudioSourcePitch, throttleMotorPower);
            _motorsAudioSource.pitch = newPitchValue;
            float newVolumeValue = Mathf.Lerp(_minMotorsAudioSourceVolume, _maxMotorsAudioSourceVolume, throttleMotorPower);
            _motorsAudioSource.volume = newVolumeValue;
        }
    }
}
