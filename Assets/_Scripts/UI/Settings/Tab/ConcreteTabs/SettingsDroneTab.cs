using UnityEngine;
using System;

public class SettingsDroneTab : SettingsTab
{
    [SerializeField] private EnumToSettingsOptionController<PlayerSettingsSO.DroneFlightModeType> _flightModeController;  
    [SerializeField] private IntToSettingsOptionController _fovController;

    public override void SaveConcretePlayerSettings()
    {
        PlayerSettingsSO.DroneFlightMode =  GetAdjustedEnumValue<PlayerSettingsSO.DroneFlightModeType>(_flightModeController.Controller);
        PlayerSettingsSO.FOV = GetAdjustedIntValue(_fovController.Controller.CurrentValue, _fovController.MinValue, _fovController.MaxValue);
    }
}
