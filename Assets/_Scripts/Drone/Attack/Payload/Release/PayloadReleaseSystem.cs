using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using Zenject;

public class PayloadReleaseSystem : MonoBehaviour
{
    [SerializeField] private DroneMovementSystem _droneMovementSystem;

    private IPayload[] _payloads;
    private IPayloadReleaseInvoker _payloadReleasable;

    [Inject]
    private void Construct(IPayloadReleaseInvoker payload)
    {
        _payloadReleasable = payload;
    }

    private void Awake()
    {
        _payloadReleasable.OnReleaseCalled += HandleBombReleaseCall;
        _payloads = GetAllPayloads();
    }

    private void HandleBombReleaseCall()
    {
        DropPayload();
    }

    private void DropPayload()
    {
        Vector3 payloadVelocityAfterDisconnetion = _droneMovementSystem.Velocity;
        foreach (IPayload payload in _payloads) 
        {
            payload.DisconnectWithVelocity(payloadVelocityAfterDisconnetion);
        }
    }

    private IPayload[] GetAllPayloads()
    { 
        return gameObject.GetComponentsInChildren<IPayload>();
    }

    private void OnDisable()
    {
        _payloadReleasable.OnReleaseCalled -= HandleBombReleaseCall;
    }
}
