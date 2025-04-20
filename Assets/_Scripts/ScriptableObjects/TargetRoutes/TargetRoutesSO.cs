using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TargetRoutes", menuName = "ScriptableObjects/TargetRoutes")]
public class TargetRoutesSO : ScriptableObject
{
    [SerializeField] private Transform _wayPointsTransforms;
}
