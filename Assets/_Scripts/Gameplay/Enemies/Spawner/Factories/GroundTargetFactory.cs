using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GroundTargetFactory : TargetFactory<GroundTarget>
{
    private GroundTarget _groundTargetPrefab;

    public GroundTargetFactory(
        GameSettingsSO gameSettingsSO,
        DifficultyLevelTargetSettingsSO difficultyLevelTargetSettingsSO,
        GroundTarget groundTargetPrefab) : base(gameSettingsSO, difficultyLevelTargetSettingsSO)
    {
        _groundTargetPrefab = groundTargetPrefab;
    }

    public override GroundTarget Create(
        Vector3 position,
        Quaternion rotation,
        Transform parent)
    {
        GroundTarget airborneTarget = GameObject.Instantiate(_groundTargetPrefab, position, rotation, parent);
        //Debug.Log($"{GetType().Name} spawned with speedMultiplier: {_targetSettings.SpeedMultiplier}");
        return airborneTarget;
    }
}
