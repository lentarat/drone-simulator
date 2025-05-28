using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/General/PlayerSettings")]
public class PlayerSettingsSO : ScriptableObject
{
    public LanguageType Language { get; set; } = LanguageType.English;
    public DroneFlightModeType DroneFlightMode { get; set; } = DroneFlightModeType.Acro;
    public int TiltAngle { get; set; } = 60;
    public int CameraAngle { get; set; }
    public int Music { get; set; } = 50;
    public int Sound { get; set; } = 50;
    public ToggleType MusicInGame { get; set; } = ToggleType.Disabled;

    public enum ToggleType
    { 
        None,
        Enabled,
        Disabled
    }

    public enum LanguageType
    { 
        None,
        English,
        Ukrainian
    }

    public enum DroneFlightModeType
    { 
        None,
        Acro,
        Angle,
        Horizon
    }
}
