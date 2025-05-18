using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneTimeRewindController : MonoBehaviour
{
    [SerializeField] private int _recordRate = 20;
    [SerializeField] private float _travelTime = 2f;

    private bool _isRecording = true;
    private int _recordIntervalMS;
    private FixedSizeQueue<DroneTimeRewindData> _fixedSizeQueue;


    //rigidbody + motors
    

    [ContextMenu("Rewind")]
    public void Rewind()
    { 
        DroneTimeRewindData data = _fixedSizeQueue.Dequeue();
        transform.position = data.Position;
        transform.rotation = data.Rotation;
    }

    private void Start()
    {
        InitializeQueue();
        CalculateRecordIntervalMS();
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

    private async UniTask RecordDataLoopAsync()
    {
        while (_isRecording)
        {
            RecordData();
            await UniTask.Delay(_recordIntervalMS);
        }
    }

    private void RecordData()
    {
        DroneTimeRewindData data = new DroneTimeRewindData(transform.position, transform.rotation); 
        _fixedSizeQueue.Enqueue(data);
    }
}
