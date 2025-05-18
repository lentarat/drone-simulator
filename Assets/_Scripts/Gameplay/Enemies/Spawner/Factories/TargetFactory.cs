using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class TargetFactory<T> where T : Target
{
    protected DifficultyLevelTargetSettingsHolder _targetSettings;
    protected T TargetPrefab { private get; set; }

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

    public virtual T Create(
        TargetSpawnData targetSpawnData,
        TargetRouteData targetRouteData,
        AudioController audioController)
    {
        T target = GameObject.Instantiate(TargetPrefab, targetSpawnData.Position, targetSpawnData.Rotation, targetSpawnData.Parent);
        target.RouteData = targetRouteData;
        target.SetAudioController(audioController);
        //Debug.Log($"{GetType().Name} spawned with speedMultiplier: {_targetSettings.SpeedMultiplier}");
        return target;
    }
}
