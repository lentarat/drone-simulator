using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class DroneHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _throttleValueText;
    [SerializeField] private TextMeshProUGUI _droneFlightModeTypeText;
    [SerializeField] private TextMeshProUGUI _droneHeightValueText;
    [SerializeField] private int _updateHUDIntervalMS;

    private bool _isInitialized;
    private bool _isUpdatingHUD = true;
    private PlayerSettingsSO.DroneFlightModeType _currentDroneFlightMode;
    private DroneMovementSystem _droneMovementSystem;
    private DroneAltimeter _droneAltimeter;
    private SignalBus _signalBus;

    [Inject]
    private void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
        _signalBus.Subscribe<PlayerSettingsChangedSignal>(HandlePlayerSettingsChangedSignal);
    }

    private void HandlePlayerSettingsChangedSignal(PlayerSettingsChangedSignal signal)
    {
        PlayerSettingsSO.DroneFlightModeType newDroneFlightMode = signal.PlayerSettingsSO.DroneFlightMode;

        bool hasDroneFlightModeChanged = HasDroneFlightModeChanged(_currentDroneFlightMode, newDroneFlightMode);
        if (hasDroneFlightModeChanged == false && _isInitialized)
        {
            return;
        }
        _currentDroneFlightMode = newDroneFlightMode;

        UpdateDroneFlightModeType(newDroneFlightMode);

        _isInitialized = true;
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

    private void UpdateDroneFlightModeType(PlayerSettingsSO.DroneFlightModeType droneFlightModeType)
    {
        _droneFlightModeTypeText.text = droneFlightModeType.ToString();
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<PlayerSettingsChangedSignal>(HandlePlayerSettingsChangedSignal);
    }

    public void Init(DroneMovementSystem droneMovementSystem, DroneAltimeter droneAltimeter)
    { 
        _droneMovementSystem = droneMovementSystem;
        _droneAltimeter = droneAltimeter;
    }

    private void Start()
    {
        UpdateHudLoopAsync().Forget();
    }

    private async UniTask UpdateHudLoopAsync()
    {
        while (_isUpdatingHUD)
        { 
            UpdateThrottleValue();
            UpdateHeightValue();    

            await UniTask.Delay(_updateHUDIntervalMS);
        }
    }

    private void UpdateThrottleValue()
    {
        int currentThrottlePercentage = (int)(_droneMovementSystem.GetThrottleMotorPowerNormalized() * 100f);
        _throttleValueText.text = currentThrottlePercentage.ToString() + "%";
    }

    private void UpdateHeightValue()
    {
        float height = Mathf.RoundToInt(_droneAltimeter.HeightValue);
        _droneHeightValueText.text = height.ToString() + "m";
    }
}
