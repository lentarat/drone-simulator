using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GroundTargetFactory : ITargetFactory<GroundTarget>
{
    private GroundTarget _groundTargetPrefab;
    private DifficultyLevelType _difficultyLevelType;
    //private DifficultyLevelTargetSpawnerSettingsSO _difficultyLevelTargetSpawnerSettingsSO;

    public GroundTargetFactory(
        GroundTarget groundTargetPrefab,
        GameSettingsSO gameSettingsSO)
    //DifficultyLevelTargetSpawnerSettingsSO difficultyLevelTargetSpawnerSettingsSO)
    {
        _groundTargetPrefab = groundTargetPrefab;
        _difficultyLevelType = gameSettingsSO.DifficultyLevelType;
        //_difficultyLevelTargetSpawnerSettingsSO = difficultyLevelTargetSpawnerSettingsSO;
    }

    GroundTarget ITargetFactory<GroundTarget>.Create(
        Vector3 position,
        Quaternion rotation,
        Transform parent)
    {
        GroundTarget groundTarget = GameObject.Instantiate(_groundTargetPrefab, position, rotation, parent);
        Debug.Log(_difficultyLevelType + GetType().Name);
        return groundTarget;
    }

    //private float GetTargetSpeedRegardingDifficulty(DifficultyLevelType)
    //{ 
        
    //}
}
