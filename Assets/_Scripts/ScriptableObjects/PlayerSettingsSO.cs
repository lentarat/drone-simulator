using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/General/PlayerSettings")]
public class PlayerSettingsSO : ScriptableObject
{
    public LanguageType Language { get; set; }
    public ToggleType HUD { get; set; }
    public DroneFlightModeType DroneFlightMode { get; set; }
    public int FOV { get; set; }
    public int CameraAngle { get; set; }
    public ToggleType AnalogCamera { get; set; }
    public ToggleType SignalLoss { get; set; }

    public enum ToggleType
    { 
        Enabled,
        Disabled
    }

    public enum LanguageType
    { 
        Ukrainian,
        English
    }

    public enum DroneFlightModeType
    { 
        Acro,
        Angle,
        Horizon
    }
}
