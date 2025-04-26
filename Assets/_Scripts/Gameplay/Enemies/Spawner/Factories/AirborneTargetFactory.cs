using UnityEngine;

public class AirborneTargetFactory : TargetFactory<AirborneTarget>
{
    private AirborneTarget _airborneTargetPrefab;

    public AirborneTargetFactory(
        GameSettingsSO gameSettingsSO,
        DifficultyLevelTargetSettingsSO difficultyLevelTargetSettingsSO,
        AirborneTarget airborneTargetPrefab) : base(gameSettingsSO, difficultyLevelTargetSettingsSO)
    {
        _airborneTargetPrefab = airborneTargetPrefab;
    }

    public override AirborneTarget Create(
        Vector3 position,
        Quaternion rotation,
        Transform parent)
    {
        AirborneTarget airborneTarget = GameObject.Instantiate(_airborneTargetPrefab, position, rotation, parent);
        Debug.Log($"{GetType().Name} spawned with speedMultiplier: {_targetSettings.SpeedMultiplier}");
        return airborneTarget;
    }
}
