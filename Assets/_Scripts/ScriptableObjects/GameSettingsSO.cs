using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings")]
public class GameSettingsSO : ScriptableObject
{
    public GameModeType GameModeType { get; set; } = GameModeType.GroundTargets;
    public DifficultyLevelType DifficultyLevelType { get; set; }
}