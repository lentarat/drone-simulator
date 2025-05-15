using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class DroneHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _throttleValueText;
    [SerializeField] private TextMeshProUGUI _droneFlightModeTypeText;
    [SerializeField] private DroneMovementSystem _droneMovementSystem;
    
    private SignalBus _playerSettingsSignalBus;

    [Inject]
    private void Construct(SignalBus playerSettingsSignalBus)
    {
        _playerSettingsSignalBus = playerSettingsSignalBus;
        //_droneMovementSystem = droneMovementSystem;
    }

    private void Awake()
    {
        _playerSettingsSignalBus.Subscribe<PlayerSettingsChangedSignal>(HandlePlayerSettingsChanged);
    }

    private void HandlePlayerSettingsChanged(PlayerSettingsChangedSignal signal)
    {
        UpdateDroneFlightModeType(signal.PlayerSettingsSO.DroneFlightMode);
    }

    private void UpdateDroneFlightModeType(PlayerSettingsSO.DroneFlightModeType droneFlightModeType)
    {
        Debug.Log("Current flight mode: " + droneFlightModeType);
    }

    private void OnDestroy()
    {
        _playerSettingsSignalBus.Unsubscribe<PlayerSettingsChangedSignal>(HandlePlayerSettingsChanged);
    }

    private void Update()
    {
        UpdateHud();    
    }

    private void UpdateHud()
    {
        UpdateThrottleValue();    
    }

    private void UpdateThrottleValue()
    {
        int currentThrottlePercentage = (int)(_droneMovementSystem.GetThrottleMotorPowerNormalized() * 100f);
        _throttleValueText.text = currentThrottlePercentage.ToString() + "%";
    }
}
