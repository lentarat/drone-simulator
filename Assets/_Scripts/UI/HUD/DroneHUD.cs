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
    [SerializeField] private DroneMovementSystem _droneMovementSystem;
    [SerializeField] private DroneAltimeter _droneAltimeter;
    [SerializeField] private int _updateHUDIntervalMS;

    private bool _isUpdatingHUD = true;
    private SignalBus _signalBus;

    [Inject]
    private void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
        _signalBus.Subscribe<PlayerSettingsChangedSignal>(HandlePlayerSettingsChanged);
    }

    private void HandlePlayerSettingsChanged(PlayerSettingsChangedSignal signal)
    {
        UpdateDroneFlightModeType(signal.PlayerSettingsSO.DroneFlightMode);
    }

    private void UpdateDroneFlightModeType(PlayerSettingsSO.DroneFlightModeType droneFlightModeType)
    {
        _droneFlightModeTypeText.text = droneFlightModeType.ToString();
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<PlayerSettingsChangedSignal>(HandlePlayerSettingsChanged);
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
