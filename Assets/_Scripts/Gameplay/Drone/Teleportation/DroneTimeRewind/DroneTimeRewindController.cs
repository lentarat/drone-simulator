using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DroneTimeRewindController : MonoBehaviour
{
    [SerializeField] private Explosive _droneExplosive;
    [SerializeField] private int _recordRate = 20;
    [SerializeField] private float _travelTime = 2f;
    [SerializeField] private Rigidbody _droneRigidbody;

    private int _recordIntervalMS;
    private FixedSizeQueue<DroneTimeRewindData> _fixedSizeQueue;

    private void Start()
    {
        InitializeQueue();
        CalculateRecordIntervalMS();
        SubscribeToDroneExplosion();
        RecordDataLoopAsync().Forget();
    }

    private void InitializeQueue()
    {
        int maxQueueMax = Mathf.CeilToInt(_recordRate * _travelTime);
        _fixedSizeQueue = new(maxQueueMax);
    }

    private void CalculateRecordIntervalMS()
    {
        float recordIntervalSeconds = 1f / _recordRate;
        _recordIntervalMS = (int)TimeSpan.FromSeconds(recordIntervalSeconds).TotalMilliseconds;
    }

    private void SubscribeToDroneExplosion()
    {
        _droneExplosive.OnExploded += RewindDrone;
    }

    private void RewindDrone()
    {
        RewindDroneAsync().Forget();
    }

    private async UniTask RewindDroneAsync()
    {
        await UniTask.WaitForFixedUpdate();

        DroneTimeRewindData data = _fixedSizeQueue.Dequeue();
        transform.position = data.Position;
        transform.rotation = data.Rotation;
        _droneRigidbody.velocity = data.RigidbodyVelocity;
        _droneRigidbody.angularVelocity = Vector3.zero;
    }

    private async UniTask RecordDataLoopAsync()
    {
        CancellationToken cancellationToken = this.GetCancellationTokenOnDestroy();

        try
        {
            while (cancellationToken.IsCancellationRequested == false)
            {
                RecordData();
                await UniTask.Delay(_recordIntervalMS);
            }
        }
        catch(OperationCanceledException e)
        {
            Debug.Log("Drone record data loop OperationCanceledException. " + e);
        }
    }

    private void RecordData()
    {
        DroneTimeRewindData data = new DroneTimeRewindData(transform.position, _droneRigidbody.velocity, transform.rotation);
        _fixedSizeQueue.Enqueue(data);
    }

    private void OnDestroy()
    {
        UnsubscribeToDroneExplosion();
    }

    private void UnsubscribeToDroneExplosion()
    {
        _droneExplosive.OnExploded -= RewindDrone;
    }
}
