using UnityEngine;

public class AirborneTargetFactory : ITargetFactory<AirborneTarget>
{
    AirborneTarget ITargetFactory<AirborneTarget>.Create(DifficultyLevelType difficultyLevelType, Vector3 position)
    {
        Debug.Log(difficultyLevelType + GetType().Name);
        return null;
    }
}
