using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GroundTargetFactory : TargetFactory<GroundTarget>
{
    public GroundTargetFactory(
        GameSettingsSO gameSettingsSO,
        DifficultyLevelTargetSettingsSO difficultyLevelTargetSettingsSO,
        GroundTarget groundTargetPrefab) : base(gameSettingsSO, difficultyLevelTargetSettingsSO)
    {
        TargetPrefab = groundTargetPrefab;
    }

    //public override GroundTarget Create(
    //    TargetSpawnData targetSpawnData,
    //    TargetRouteData targetRouteData,
    //    AudioController audioController)
    //{
    //    GroundTarget groundTarget = GameObject.Instantiate(TargetPrefab, targetSpawnData.Position, targetSpawnData.Rotation, targetSpawnData.Parent);
    //    groundTarget.SetAudioController(audioController);
    //    //Debug.Log($"{GetType().Name} spawned with speedMultiplier: {_targetSettings.SpeedMultiplier}");
    //    return groundTarget;
    //}
}
