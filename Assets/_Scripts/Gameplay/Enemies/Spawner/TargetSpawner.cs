using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Zenject;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private Transform _targetScopePrefab;
    [SerializeField] private Transform[] _targetsTransforms;

    private int _lastTargetIndex = -1;
    private Coroutine _changeTargetsCoroutine;
    private Queue<GameObject> _targetScopesQueue = new();
    private ITargetFactory _targetFactory;

    [Inject]
    private void Construct()
    { 

    }

    private void Awake()
    {
        Invoke("ShowNewTarget", 1.25f);
    }

    private void ShowNewTarget()
    {
        if (_changeTargetsCoroutine == null)
        {
            _changeTargetsCoroutine = StartCoroutine(ChangeTargetsCoroutine());
        }

        int randomTargetIndex = GetRandomTargetsIndex();
        Vector3 randomTargetPosition = _targetsTransforms[randomTargetIndex].position;
        Ray ray = new Ray(randomTargetPosition, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Transform targetScope = Instantiate(_targetScopePrefab, transform);
            targetScope.transform.position = hitInfo.point;
            targetScope.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            targetScope.transform.position += targetScope.transform.up * 0.01f;
            _targetScopesQueue.Enqueue(targetScope.gameObject);
        }
    }

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

        if (_targetsTransforms.Length == 0)
        {
            throw new System.Exception("Targets transforms array is empty");
        }

        if (_targetsTransforms.Length > 1)
        {
            do
            {
                randomTargetIndex = Random.Range(0, _targetsTransforms.Length);
                Debug.Log("New random index: " + randomTargetIndex);
            } while (randomTargetIndex == _lastTargetIndex);
        }
        else
        {
            randomTargetIndex = 0;
        }

        return randomTargetIndex;
    }

    private void HideAllTargets()
    {
        StopCoroutine(_changeTargetsCoroutine);
        Debug.Log("Stopped Coroutine " + _changeTargetsCoroutine);
        _changeTargetsCoroutine = null;
    }
}
