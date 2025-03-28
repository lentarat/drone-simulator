using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings")]
public class GameSettingsSO : ScriptableObject
{
    [SerializeField] private TargetMovementType _targetMovementType;
    public TargetMovementType TargetMovementType => _targetMovementType;
    [SerializeField] private int _difficultyLevel;
    public int DifficultyLevel => _difficultyLevel;
}

public enum TargetMovementType
{ 
    Ground,
    Airborne
}
