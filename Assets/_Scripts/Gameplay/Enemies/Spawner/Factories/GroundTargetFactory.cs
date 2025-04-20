using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GroundTargetFactory : ITargetFactory<GroundTarget>
{
    private GroundTarget _groundTargetPrefab;

    public GroundTargetFactory(GroundTarget groundTargetPrefab)
    {
        _groundTargetPrefab = groundTargetPrefab;
    }

    GroundTarget ITargetFactory<GroundTarget>.Create(DifficultyLevelType difficultyLevelType, Vector3 position, Quaternion rotation, Transform parent)
    {
        GroundTarget groundTarget = GameObject.Instantiate(_groundTargetPrefab, position, rotation, parent);
        groundTarget.transform.position += groundTarget.transform.up * 0.01f;
        Debug.Log(difficultyLevelType + GetType().Name);
        return groundTarget;
    }
}
