using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyLevelTargetSettingSO", menuName = "ScriptableObjects/DifficultyLevelTargetSettingSO")]
public class DifficultyLevelTargetSettingSO : ScriptableObject
{
    [SerializeField] private DifficultyLevelTargetSettingsHolder[] _difficultyLevelTargetSettingsHolder;
    public DifficultyLevelTargetSettingsHolder[] DifficultyLevelTargetSettingsHolder => _difficultyLevelTargetSettingsHolder;
}

[Serializable]
public struct DifficultyLevelTargetSettingsHolder
{
    [SerializeField] private DifficultyLevelType _difficultyLevelType;
    public DifficultyLevelType DifficultyLevelType => _difficultyLevelType;
    [SerializeField] private int _speedMultiplier;
    public int SpeedMultiplier => _speedMultiplier;
}