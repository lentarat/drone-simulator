using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using Zenject;

public class PayloadReleaseSystem : MonoBehaviour
{
    [SerializeField] private DroneMovementSystem _droneMovementSystem;

    private Payload[] _payloads;
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
        //StartCoroutine(RespawnPayload());
        DropPayload();
    }

    private void DropPayload()
    {
        Vector3 payloadVelocityAfterDisconnetion = _droneMovementSystem.Velocity;
        foreach (Payload payload in _payloads) 
        {
            payload.DisconnectWithVelocity(payloadVelocityAfterDisconnetion);
        }
    }

    //private IEnumerator RespawnPayload()
    //{
    //    foreach (Payload payload in _payloads)
    //    {
    //        Instantiate(payload, transform);
    //    }
    //}

    private Payload[] GetAllPayloads()
    { 
        return gameObject.GetComponentsInChildren<Payload>();
    }

    private void OnDisable()
    {
        _payloadReleasable.OnReleaseCalled -= HandleBombReleaseCall;
    }
}
