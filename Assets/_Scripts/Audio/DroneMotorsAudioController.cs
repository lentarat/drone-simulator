using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMotorsAudioController : MonoBehaviour
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
        float yawMotorPower = _droneMovementSystem.GetYawMotorPowerNormalized();
        float pitchAndRollMotorPower = _droneMovementSystem.GetPitchAndRollMotorPowerNormalized();
        float highestMotorPowerValue = Mathf.Max(throttleMotorPower, yawMotorPower, pitchAndRollMotorPower);
        if (highestMotorPowerValue > 0)
        {
            float newPitchValue = Mathf.Lerp(_minMotorsAudioSourcePitch, _maxMotorsAudioSourcePitch, highestMotorPowerValue);
            _motorsAudioSource.pitch = newPitchValue;
            float newVolumeValue = Mathf.Lerp(_minMotorsAudioSourceVolume, _maxMotorsAudioSourceVolume, highestMotorPowerValue);
            _motorsAudioSource.volume = newVolumeValue;
        }
    }
}
