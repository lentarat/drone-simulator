using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsSO : MonoBehaviour
{
    public TargetMovementType TargetMovementType { get; set; }
    public int DifficultyLevel { get; set; }
}

public enum TargetMovementType
{ 
    Ground,
    Airborne
}
