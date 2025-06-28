using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DroneCameraAdjuster : MonoBehaviour
{
    [SerializeField] private Camera _frontCamera;

    private int? _currentCameraAngle = null;
    private int? _currentCameraFOV = null;
    private SignalBus _signalBus;

    [Inject]
    private void Construct(SignalBus signalBus)
    { 
        _signalBus = signalBus;
        SubscribeToPlayerSettingsChangedSignal();
    }

    private void SubscribeToPlayerSettingsChangedSignal()
    {
        _signalBus.Subscribe<PlayerSettingsChangedSignal>(HandlePlayerSettingsChangedSignal);
    }

    private void HandlePlayerSettingsChangedSignal(PlayerSettingsChangedSignal playerSettingsChangedSignal)
    {
        HandleCameraAngle(playerSettingsChangedSignal);
        HandleCameraFOV(playerSettingsChangedSignal);
    }

    private void HandleCameraAngle(PlayerSettingsChangedSignal playerSettingsChangedSignal)
    {
        int cameraAngle = playerSettingsChangedSignal.PlayerSettingsSO.CameraAngle;
        bool hasCameraAngleChanged = HasValueChanged(cameraAngle, _currentCameraAngle);
        if (hasCameraAngleChanged)
        {
            SetNewFrontCameraAngle(cameraAngle);
            _currentCameraAngle = cameraAngle;
        }
    }

    private bool HasValueChanged(int newAngle, int? oldAngle)
    {
        if (newAngle == oldAngle)
        {
            return false;
        }

        return true;
    }

    private void SetNewFrontCameraAngle(int newAngle)
    {
        Vector3 eulerRotation = _frontCamera.transform.localRotation.eulerAngles;
        eulerRotation.x = newAngle;
        _frontCamera.transform.localRotation = Quaternion.Euler(eulerRotation);
    }

    private void HandleCameraFOV(PlayerSettingsChangedSignal playerSettingsChangedSignal)
    {
        int cameraFOV = playerSettingsChangedSignal.PlayerSettingsSO.CameraFOV;
        bool hasCameraFOVChanged = HasValueChanged(cameraFOV, _currentCameraFOV);
        if (hasCameraFOVChanged)
        {
            SetNewFrontCameraFOV(cameraFOV);
            _currentCameraFOV = cameraFOV;
        }
    }

    private void SetNewFrontCameraFOV(int newFOV)
    {
        _frontCamera.fieldOfView = newFOV;
    }

    private void OnDestroy()
    {
        UnsubscribeToPlayerSettingsChangedSignal();
    }

    private void UnsubscribeToPlayerSettingsChangedSignal()
    {
        _signalBus.Unsubscribe<PlayerSettingsChangedSignal>(HandlePlayerSettingsChangedSignal);
    }
}
