using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GroundTargetFactory : ITargetFactory<GroundTarget>
{
    GroundTarget ITargetFactory<GroundTarget>.Create(DifficultyLevelType difficultyLevelType, Vector3 position)
    {
        Debug.Log(difficultyLevelType + GetType().Name);
        return null;
    }
}
