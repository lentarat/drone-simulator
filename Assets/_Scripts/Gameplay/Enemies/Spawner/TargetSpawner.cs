using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Zenject;

public class TargetSpawner<T> : IInitializable where T : Target
{
    private bool _isSpawning;
    private readonly Vector3[][] _routesPositions;
    private readonly Transform _targetsParent;
    private readonly AudioController _audioController;
    private readonly HashSet<T> _targetsHashSet = new();
    private readonly TargetFactory<T> _targetFactory;
    private readonly DifficultyLevelTargetSpawnerSettingsHolder _targetSpawnerSettings;

    public TargetSpawner(
        TargetFactory<T> targetFactory,
        AudioController audioController,
        GameSettingsSO gameSettingsSO,
        DifficultyLevelTargetSpawnerSettingsSO difficultyLevelTargetSpawnerSettingsSO,
        [Inject(Id = "TargetsParent")] Transform targetsParent,
        [Inject(Id = "RoutesParents")] GameObject[] routesParents)
    {
        _targetFactory = targetFactory;
        _audioController = audioController;
        _targetsParent = targetsParent;
        _routesPositions = RouteUtils.GetConvertedRoutesWaypointsPositions(routesParents);
        _targetSpawnerSettings =
            GetSettingsRegardingDifficultyLevel(gameSettingsSO.DifficultyLevelType, difficultyLevelTargetSpawnerSettingsSO);
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
            bool isTargetCountExceeded = IsTargetCountExceeded(_targetsHashSet.Count, _targetSpawnerSettings.MaxTargetsCount);
            if (isTargetCountExceeded)
            {
                int nextCheckIntervalMS = 300;
                await UniTask.Delay(nextCheckIntervalMS);
                continue;
            }
            SpawnTarget();

            await UniTask.Delay(_targetSpawnerSettings.NextTargetSpawnIntervalMS);
        }
    }

    private bool IsTargetCountExceeded(int currentTargetCount, int maxTargetCount)
    {
        if (currentTargetCount >= maxTargetCount)
        {
            return true;
        }

        return false;
    }

    private void SpawnTarget()
    {
        int randomRouteIndex = Random.Range(0, _routesPositions.Length);
        int randomWaypointPositionIndex = Random.Range(0, _routesPositions[randomRouteIndex].Length);
        Vector3 randomRouteWaypointPosition = _routesPositions[randomRouteIndex][randomWaypointPositionIndex];

        Ray ray = new Ray(randomRouteWaypointPosition, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 spawnPosition = hitInfo.point;
            Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            TargetSpawnData targetSpawnData = new(spawnPosition, spawnRotation, _targetsParent);

            Vector3[] routeWaypointPositions = _routesPositions[randomRouteIndex];
            TargetRouteData targetRouteData = new(routeWaypointPositions, randomWaypointPositionIndex);

            Target target = _targetFactory.Create(targetSpawnData, targetRouteData, _audioController);
            target.OnDeathAction = () => RemoveTargetFromHashSet(target);

            _targetsHashSet.Add((T)target);
        }
    }

    private void RemoveTargetFromHashSet(Target target)
    {
        _targetsHashSet.Remove((T)target);
    }
}

