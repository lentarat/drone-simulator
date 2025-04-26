using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class TargetFactory<T> where T : Target
{
    protected DifficultyLevelTargetSettingsHolder _targetSettings;

    protected TargetFactory(
        GameSettingsSO gameSettingsSO,
        DifficultyLevelTargetSettingsSO difficultyLevelTargetSettingsSO)
    {
        _targetSettings = GetSettingsRegardingDifficultyLevel(gameSettingsSO.DifficultyLevelType, difficultyLevelTargetSettingsSO);
    }

    private DifficultyLevelTargetSettingsHolder GetSettingsRegardingDifficultyLevel(
        DifficultyLevelType difficultyLevelType,
        DifficultyLevelTargetSettingsSO difficultyLevelTargetSettingsSO)
    {
        return difficultyLevelTargetSettingsSO.DifficultyLevelTargetSettingsHolder
            .FirstOrDefault(e => e.DifficultyLevelType == difficultyLevelType);
    }

    public abstract T Create(
        Vector3 position,
        Quaternion rotation,
        Transform parent);
}
