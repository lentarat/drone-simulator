using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TargetSpawner<T> : IInitializable where T : Target
{
    private bool _isSpawning;
    private int _lastTargetIndex = -1;
    private int _maxTargetsNumber = 5;
    private Transform _targetsParent;
    //private Transform[] _targetSpawnTransforms;
    private Vector3[][] _routesPositions;
    private Queue<GameObject> _targetsQueue = new();
    private DifficultyLevelType _difficultyLevelType;
    private ITargetFactory<T> _targetFactory;

    public TargetSpawner(
        ITargetFactory<T> targetFactory,
        GameSettingsSO gameSettingsSO,
        [Inject(Id = "TargetsParent")] Transform targetsParent,
        [Inject(Id = "RoutesParents")] GameObject[] routesParents)
    {
        _difficultyLevelType = gameSettingsSO.DifficultyLevelType;
        _targetFactory = targetFactory;
        _targetsParent = targetsParent;
        _routesPositions = RouteUtils.GetConvertedRoutesWaypointsPositions(routesParents);
        //_maxTargetsNumber = (int)(_targetSpawnTransforms.Length * 1.5f);
    }

    private void SpawnTarget()
    {
        if (_targetsQueue.Count > _maxTargetsNumber)
            return;

        int randomRouteIndex = Random.Range(0, _routesPositions.Length); //GetRandomRouteIndex();
        int randomWaypointPositionIndex = Random.Range(0, _routesPositions[randomRouteIndex].Length);
        Vector3 randomRouteWaypointPosition = _routesPositions[randomRouteIndex][randomWaypointPositionIndex];
        Ray ray = new Ray(randomRouteWaypointPosition, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 spawnPosition = hitInfo.point;
            Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            Target target = _targetFactory.Create(_difficultyLevelType, spawnPosition, spawnRotation, _targetsParent);
            _targetsQueue.Enqueue(target.gameObject);
        }
    }

    //private int GetRandomRouteIndex()
    //{
    //    int randomTargetIndex;

    //    if (_targetSpawnTransforms.Length == 0)
    //    {
    //        throw new System.Exception("Target spawn transforms array is empty");
    //    }

    //    if (_targetSpawnTransforms.Length > 1)
    //    {
    //        do
    //        {
    //            randomTargetIndex = Random.Range(0, _targetSpawnTransforms.Length);
    //            Debug.Log("New random index: " + randomTargetIndex);
    //        } while (randomTargetIndex == _lastTargetIndex);
    //    }
    //    else
    //    {
    //        randomTargetIndex = 0;
    //    }

    //    return randomTargetIndex;
    //}

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

            await UniTask.Delay(1000);
        }
    }
}
