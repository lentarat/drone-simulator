using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/General/PlayerSettings")]
public class PlayerSettingsSO : ScriptableObject
{
    public LanguageType Language { get; set; }
    //public ToggleType HUD { get; set; }
    public DroneFlightModeType DroneFlightMode { get; set; }
    public int TiltAngle { get; set; } = 60;
    //public int FOV { get; set; }
    public int CameraAngle { get; set; }
    [SerializeField] public int Sound /*{ get; set; }*/ = 40;
    [SerializeField] public int Music /*{ get; set; } */= 30;
    //public ToggleType AnalogCamera { get; set; }
    //public ToggleType SignalLoss { get; set; }

    public enum ToggleType
    { 
        Enabled,
        Disabled
    }

    public enum LanguageType
    { 
        English,
        Ukrainian
    }

    public enum DroneFlightModeType
    { 
        Acro,
        Angle,
        Horizon
    }
}
