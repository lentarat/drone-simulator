using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using Zenject;

public class DronePayloadReleaseSystem : MonoBehaviour
{
    [SerializeField] private DroneMovementSystem _droneMovementSystem;
    [SerializeField] private DronePayload _payloadPrefab;

    private IPayloadReleaseInvoker _payloadReleasable;
    private DronePayload _payload;

    [Inject]
    private void Construct(IPayloadReleaseInvoker payload)
    {
        _payloadReleasable = payload;
    }

    private void Awake()
    {
        _payloadReleasable.OnReleaseCalled += HandleBombReleaseCall;
        SpawnPayload();
    }

    private void HandleBombReleaseCall()
    {
        DropPayload();
        Invoke("SpawnPayload", 3f);
    }

    private void DropPayload()
    {
        Vector3 payloadVelocityAfterDisconnetion = _droneMovementSystem.Velocity;
        _payload.DisconnectWithVelocity(payloadVelocityAfterDisconnetion);
    }

    private void SpawnPayload()
    {
        if(_payload != null ) 
        {
            throw new System.Exception("There is already a payload");
        }

        _payload = Instantiate(_payloadPrefab, transform);    
    }

    private void OnDisable()
    {
        _payloadReleasable.OnReleaseCalled -= HandleBombReleaseCall;
    }
}
