using UnityEngine;
using System;

public class SettingsDroneTab : SettingsTab
{
    [SerializeField] private EnumToSettingsOptionController<PlayerSettingsSO.DroneFlightModeType> _flightModeController;  
    [SerializeField] private IntToSettingsOptionController _fovController;

}
