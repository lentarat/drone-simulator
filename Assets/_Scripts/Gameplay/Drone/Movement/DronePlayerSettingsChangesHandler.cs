using System;
using Zenject;
using UnityEngine;

public class DronePlayerSettingsChangesHandler
{
    public event Action<DroneFlightModeMovementAdjuster> OnDronePlayerSettingsChanged;

    private int _currentTiltAngle;
    private PlayerSettingsSO.DroneFlightModeType _currentDroneFlightMode;
    private SignalBus _signalBus;
    public DroneFlightModeMovementAdjuster DroneFlightModeMovementAdjuster { get; private set; } = new DroneAcroMovementAdjuster();

    public DronePlayerSettingsChangesHandler(SignalBus signalBus)
    {
        _signalBus = signalBus;
        _signalBus.Subscribe<PlayerSettingsChangedSignal>(HandlePlayerSettingsChangedSignal);
    }

    private void HandlePlayerSettingsChangedSignal(PlayerSettingsChangedSignal signal)
    {
        HandleDroneFlightMode(signal);
        HandleTiltAngle(signal);
    }

    private void HandleDroneFlightMode(PlayerSettingsChangedSignal signal)
    {
        PlayerSettingsSO.DroneFlightModeType newDroneFlightMode = signal.PlayerSettingsSO.DroneFlightMode;

        bool hasDroneFlightModeChanged = HasDroneFlightModeChanged(_currentDroneFlightMode, newDroneFlightMode);
        if (hasDroneFlightModeChanged == false)
        {
            return;
        }
        _currentDroneFlightMode = newDroneFlightMode;

        DroneFlightModeMovementAdjuster = newDroneFlightMode switch
        {
            PlayerSettingsSO.DroneFlightModeType.Acro => new DroneAcroMovementAdjuster(),
            PlayerSettingsSO.DroneFlightModeType.Angle => new DroneAngleMovementAdjuster(),
            PlayerSettingsSO.DroneFlightModeType.Horizon => new DroneHorizonMovementAdjuster(),
            _ => new DroneAcroMovementAdjuster()
        };

        DroneFlightModeMovementAdjuster.SetTiltAngleThreshold(_currentTiltAngle);
        OnDronePlayerSettingsChanged?.Invoke(DroneFlightModeMovementAdjuster);
    }

    private bool HasDroneFlightModeChanged(
        PlayerSettingsSO.DroneFlightModeType currentDroneFlightModeType,
        PlayerSettingsSO.DroneFlightModeType newDroneFlightModeType)
    {
        if (currentDroneFlightModeType != newDroneFlightModeType)
        {
            return true;
        }

        return false;
    }

    private void HandleTiltAngle(PlayerSettingsChangedSignal signal)
    {
        int newTiltAngle = signal.PlayerSettingsSO.TiltAngle;
        bool hasTiltAngleChanged = HasTiltAngleChanged(_currentTiltAngle, newTiltAngle);
        if (hasTiltAngleChanged == false)
        {
            return;
        }

        _currentTiltAngle = newTiltAngle;
        DroneFlightModeMovementAdjuster.SetTiltAngleThreshold(newTiltAngle);
    }

    private bool HasTiltAngleChanged(int currentTiltAngle, int newTiltAngle)
    {
        if (currentTiltAngle != newTiltAngle)
        {
            return true;
        }

        return false;
    }

    ~DronePlayerSettingsChangesHandler()
    {
        _signalBus.Unsubscribe<PlayerSettingsChangedSignal>(HandlePlayerSettingsChangedSignal);
    }
}
