using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/General/GameSettings")]
public class GameSettingsSO : ScriptableObject
{
    public GameModeType GameModeType { get; set; } = GameModeType.Kamikadze;
    public DifficultyLevelType DifficultyLevelType { get; set; } = DifficultyLevelType.Medium;
}