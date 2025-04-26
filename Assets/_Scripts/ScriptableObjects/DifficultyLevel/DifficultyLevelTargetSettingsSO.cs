using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyLevelTargetSettingsSO", menuName = "ScriptableObjects/DifficultyLevel/DifficultyLevelTargetSettingsSO")]
public class DifficultyLevelTargetSettingsSO : ScriptableObject
{
    [SerializeField] private DifficultyLevelTargetSettingsHolder[] _difficultyLevelTargetSettingsHolder;
    public DifficultyLevelTargetSettingsHolder[] DifficultyLevelTargetSettingsHolder => _difficultyLevelTargetSettingsHolder;
}

[Serializable]
public struct DifficultyLevelTargetSettingsHolder
{
    [SerializeField] private DifficultyLevelType _difficultyLevelType;
    public DifficultyLevelType DifficultyLevelType => _difficultyLevelType;
    [SerializeField] private float _speedMultiplier;
    public float SpeedMultiplier => _speedMultiplier;
}