using UnityEngine;

public class AirborneTargetFactory : ITargetFactory<AirborneTarget>
{
    private AirborneTarget _airborneTargetPrefab;
    private DifficultyLevelType _difficultyLevelType;
   //private DifficultyLevelTargetSpawnerSettingsSO _difficultyLevelTargetSpawnerSettingsSO;

    public AirborneTargetFactory(
        AirborneTarget airborneTargetPrefab,
        GameSettingsSO gameSettingsSO)
        //DifficultyLevelTargetSpawnerSettingsSO difficultyLevelTargetSpawnerSettingsSO)
    {
        _airborneTargetPrefab = airborneTargetPrefab;
        _difficultyLevelType = gameSettingsSO.DifficultyLevelType;
        //_difficultyLevelTargetSpawnerSettingsSO = difficultyLevelTargetSpawnerSettingsSO;
    }

    AirborneTarget ITargetFactory<AirborneTarget>.Create(
        Vector3 position,
        Quaternion rotation,
        Transform parent)
    {
        AirborneTarget airborneTarget = GameObject.Instantiate(_airborneTargetPrefab, position, rotation, parent);
        Debug.Log(_difficultyLevelType + GetType().Name);
        return airborneTarget;
    }
}
