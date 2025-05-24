using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

public class SettingsDroneTab : SettingsTab
{
    [SerializeField] private EnumToSettingsOptionController<PlayerSettingsSO.DroneFlightModeType> _flightModeController;  
    [SerializeField] private IntToSettingsOptionController _tiltController;
    //[SerializeField] private IntToSettingsOptionController _fovController;

    public override void SaveConcretePlayerSettings()
    {
        PlayerSettingsSO.DroneFlightMode = (PlayerSettingsSO.DroneFlightModeType)_flightModeController.Controller.GetEnumCurrentValue();
        PlayerSettingsSO.TiltAngle = _tiltController.Controller.CurrentValue;
    }

    protected override async UniTask ProvideCurrentValuesToControllersAsync()
    {
        await base.ProvideCurrentValuesToControllersAsync();

        int droneFlightModeTypeCurrentValue = Convert.ToInt32(PlayerSettingsSO.DroneFlightMode);
        _flightModeController.Controller.Init<PlayerSettingsSO.DroneFlightModeType>(LocalizationTable, droneFlightModeTypeCurrentValue);

        int tiltAngleCurrentValue = PlayerSettingsSO.TiltAngle;
        _tiltController.Controller.Init(tiltAngleCurrentValue, _tiltController.MinValue, _tiltController.MaxValue);
    }
}
