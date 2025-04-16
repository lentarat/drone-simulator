using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Zenject;

public class TargetSpawner<T> where T : Target
{
    private int _lastTargetIndex = -1;
    private Coroutine _changeTargetsCoroutine;
    private Transform _targetScopePrefab;
    private Transform[] _targetSpawnPositions;
    private Queue<GameObject> _targetScopesQueue = new();
    private DifficultyLevelType _difficultyLevelType;
    private ITargetFactory<T> _targetFactory;

    public TargetSpawner(ITargetFactory<T> targetFactory, GameSettingsSO gameSettingsSO, Transform[] targetSpawnPositions, Transform targetScopePrefab)
    {
        _difficultyLevelType = gameSettingsSO.DifficultyLevelType;
        _targetFactory = targetFactory;
        _targetSpawnPositions = targetSpawnPositions;
        _targetScopePrefab = targetScopePrefab;
        new MonoBehaviour();
    }

    private void Awake()
    {
        //Invoke("ShowNewTarget", 1.25f);
    }

    //private void ShowNewTarget()
    //{
    //    if (_changeTargetsCoroutine == null)
    //    {
    //        _changeTargetsCoroutine = StartCoroutine(ChangeTargetsCoroutine());
    //    }

    //    int randomTargetIndex = GetRandomTargetsIndex();
    //    Vector3 randomTargetPosition = _targetsTransforms[randomTargetIndex].position;
    //    Ray ray = new Ray(randomTargetPosition, Vector3.down);
    //    if (Physics.Raycast(ray, out RaycastHit hitInfo))
    //    {
    //        Transform targetScope = Instantiate(_targetScopePrefab, transform);
    //        targetScope.transform.position = hitInfo.point;
    //        targetScope.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
    //        targetScope.transform.position += targetScope.transform.up * 0.01f;
    //        _targetScopesQueue.Enqueue(targetScope.gameObject);
    //    }
    //}

    private IEnumerator ChangeTargetsCoroutine()
    {
        while (true)
        {
            yield return null;
        }
    }

    private int GetRandomTargetsIndex()
    {
        int randomTargetIndex;

        if (_targetSpawnPositions.Length == 0)
        {
            throw new System.Exception("Targets transforms array is empty");
        }

        if (_targetSpawnPositions.Length > 1)
        {
            do
            {
                randomTargetIndex = Random.Range(0, _targetSpawnPositions.Length);
                Debug.Log("New random index: " + randomTargetIndex);
            } while (randomTargetIndex == _lastTargetIndex);
        }
        else
        {
            randomTargetIndex = 0;
        }

        return randomTargetIndex;
    }

    //private void HideAllTargets()
    //{
    //    StopCoroutine(_changeTargetsCoroutine);
    //    Debug.Log("Stopped Coroutine " + _changeTargetsCoroutine);
    //    _changeTargetsCoroutine = null;
    //}
}
