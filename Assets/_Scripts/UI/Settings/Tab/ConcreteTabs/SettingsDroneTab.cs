using UnityEngine;
using System;
using Cysharp.Threading.Tasks.Triggers;
using Cysharp.Threading.Tasks;

public class SettingsDroneTab : SettingsTab
{
    [SerializeField] private EnumToSettingsOptionController<PlayerSettingsSO.DroneFlightModeType> _flightModeController;  
    [SerializeField] private IntToSettingsOptionController _fovController;

    public override void SaveConcretePlayerSettings()
    {
        PlayerSettingsSO.DroneFlightMode = (PlayerSettingsSO.DroneFlightModeType)_flightModeController.Controller.GetEnumCurrentValue();
        


        
        //PlayerSettingsSO.DroneFlightMode = _flightModeController.Controller.GetAdjustedEnumValue<PlayerSettingsSO.DroneFlightModeType>();
        //PlayerSettingsSO.FOV = _fovController.Controller.GetAdjustedIntValue(_fovController.MinValue, _fovController.MaxValue);
        
        //GetAdjustedEnumValue<PlayerSettingsSO.DroneFlightModeType>(_flightModeController.Controller);
        //PlayerSettingsSO.FOV = GetAdjustedIntValue(_fovController.Controller.CurrentValue, _fovController.MinValue, _fovController.MaxValue);
    }

    protected override async UniTask ProvideCurrentValuesToControllers()
    {
        await base.ProvideCurrentValuesToControllers();

        int droneFlightModeCurrentValue = Convert.ToInt32(PlayerSettingsSO.DroneFlightMode);
        _flightModeController.Controller.Init<PlayerSettingsSO.DroneFlightModeType>(LocalizationTable, droneFlightModeCurrentValue);
        //int adjustedFOVValue = _fovController.Controller.GetAdjustedIntValue(_fovController.MinValue, _fovController.MaxValue);
        //_fovController.Controller.Init(adjustedFOVValue);
    }
}
