using UnityEngine;

public class AirborneTargetFactory : ITargetFactory<AirborneTarget>
{
    private AirborneTarget _airborneTargetPrefab;

    public AirborneTargetFactory(AirborneTarget airborneTargetPrefab)
    {
        _airborneTargetPrefab = airborneTargetPrefab;
    }

    AirborneTarget ITargetFactory<AirborneTarget>.Create(DifficultyLevelType difficultyLevelType, Vector3 position, Quaternion rotation, Transform parent)
    {
        AirborneTarget airborneTarget = GameObject.Instantiate(_airborneTargetPrefab, position, rotation, parent);
        airborneTarget.transform.position += airborneTarget.transform.up * 0.01f;
        Debug.Log(difficultyLevelType + GetType().Name);
        return airborneTarget;
    }
}
