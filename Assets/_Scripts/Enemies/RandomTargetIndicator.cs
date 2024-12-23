using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTargetIndicator : MonoBehaviour
{
    [SerializeField] private Transform[] _targetsTransforms;
    [SerializeField] private Transform _targetScopePrefab;

    private int _lastTargetIndex = -1;

    private void Awake()
    {
        Invoke("ShowNewTarget", 1.25f);
    }

    private void ShowNewTarget()
    {
        int randomTargetIndex = GetRandomTargetsIndex();
        Vector3 randomTargetPosition = _targetsTransforms[randomTargetIndex].position;
        Ray ray = new Ray(randomTargetPosition, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Debug.Log($"Position: {hitInfo.point}, normal direction: {hitInfo.normal}");
            Transform targetScope = Instantiate(_targetScopePrefab, transform);
            targetScope.transform.position = hitInfo.point;
            targetScope.transform.rotation = Quaternion.LookRotation(-hitInfo.normal,Vector3.down);
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
}
