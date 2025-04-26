using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class TargetSpawner<T> : IInitializable where T : Target
{
    private bool _isSpawning;
    //private int _maxTargetsNumber = 5;
    private Transform _targetsParent;
    private Vector3[][] _routesPositions;
    private Queue<GameObject> _targetsQueue = new();
    private ITargetFactory<T> _targetFactory;
    private DifficultyLevelTargetSpawnerSettingsHolder _targetSpawnerSettings;

    public TargetSpawner(
        ITargetFactory<T> targetFactory,
        GameSettingsSO gameSettingsSO,
        DifficultyLevelTargetSpawnerSettingsSO difficultyLevelTargetSpawnerSettingsSO,
        [Inject(Id = "TargetsParent")] Transform targetsParent,
        [Inject(Id = "RoutesParents")] GameObject[] routesParents)
    {
        _targetFactory = targetFactory;
        _targetsParent = targetsParent;
        _routesPositions = RouteUtils.GetConvertedRoutesWaypointsPositions(routesParents);
        _targetSpawnerSettings =
            GetSettingsRegardingDifficultyLevel(gameSettingsSO.DifficultyLevelType, difficultyLevelTargetSpawnerSettingsSO);
        Debug.Log(_targetSpawnerSettings.DifficultyLevelType + " " + _targetSpawnerSettings.NextTargetSpawnIntervalMS);
        Debug.Break();
        //_maxTargetsNumber = (int)(_targetSpawnTransforms.Length * 1.5f);
    }

    private DifficultyLevelTargetSpawnerSettingsHolder GetSettingsRegardingDifficultyLevel(
        DifficultyLevelType difficultyLevelType,
        DifficultyLevelTargetSpawnerSettingsSO difficultyLevelTargetSpawnerSettingsSO)
    {
        return difficultyLevelTargetSpawnerSettingsSO.DifficultyLevelTargetSpawnerSettingsHolder
            .FirstOrDefault(e => e.DifficultyLevelType == difficultyLevelType);
    }

    void IInitializable.Initialize()
    {
        StartSpawning().Forget();
    }

    private async UniTask StartSpawning()
    {
        _isSpawning = true;

        while (_isSpawning)
        {
            SpawnTarget();

            await UniTask.Delay(5000);
        }
    }

    private void SpawnTarget()
    {
        //if (_targetsQueue.Count > _maxTargetsNumber)
        //    return;

        int randomRouteIndex = Random.Range(0, _routesPositions.Length);
        int randomWaypointPositionIndex = Random.Range(0, _routesPositions[randomRouteIndex].Length);
        Vector3 randomRouteWaypointPosition = _routesPositions[randomRouteIndex][randomWaypointPositionIndex];
        Ray ray = new Ray(randomRouteWaypointPosition, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 spawnPosition = hitInfo.point;
            Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            Target target = _targetFactory.Create(spawnPosition, spawnRotation, _targetsParent);
            target.Init(_routesPositions[randomRouteIndex], randomWaypointPositionIndex);
            _targetsQueue.Enqueue(target.gameObject);
        }
    }
}
