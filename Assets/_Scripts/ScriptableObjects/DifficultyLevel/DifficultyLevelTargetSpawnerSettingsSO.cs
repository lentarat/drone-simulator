using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyLevelTargetSpawnerSettingsHolder", menuName = "ScriptableObjects/DifficultyLevel/DifficultyLevelTargetSpawnerSettingsHolder")]
public class DifficultyLevelTargetSpawnerSettingsSO : ScriptableObject
{
    [SerializeField] private DifficultyLevelTargetSpawnerSettingsHolder[] _difficultyLevelTargetSpawnerSettingsHolder;
    public DifficultyLevelTargetSpawnerSettingsHolder[] DifficultyLevelTargetSpawnerSettingsHolder => _difficultyLevelTargetSpawnerSettingsHolder;
}

[Serializable]
public struct DifficultyLevelTargetSpawnerSettingsHolder
{
    [SerializeField] private DifficultyLevelType _difficultyLevelType;
    [SerializeField] private int _nextTargetSpawnIntervalMS;
    [SerializeField] private int _maxTargetsCount;
    public DifficultyLevelType DifficultyLevelType => _difficultyLevelType;
    public int NextTargetSpawnIntervalMS => _nextTargetSpawnIntervalMS;
    public int MaxTargetsCount => _maxTargetsCount;
}