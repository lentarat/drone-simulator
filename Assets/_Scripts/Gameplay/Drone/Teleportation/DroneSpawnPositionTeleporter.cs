using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

public class DroneSpawnPositionTeleporter : MonoBehaviour
{
    [SerializeField] private Rigidbody _droneRigidbody;
    [SerializeField] private DronePayloadReleaseSystem _dronePayloadReleaseSystem;
    [SerializeField] private Rigidbody _payloadRigidbody;

    private Transform _spawnPositionTransform;
    private InputActions _inputActions;

    [Inject]
    private void Construct(InputActions inputActions)
    {
        _inputActions = inputActions;
        SubscribeToTeleportButtonPerformed();
    }

    private void SubscribeToTeleportButtonPerformed()
    {
        _inputActions.Drone.TeleportToSpawnPosition.performed += HandleTeleportButtonPerformed;
    }

    private void HandleTeleportButtonPerformed(InputAction.CallbackContext context)
    {
        TeleportDroneAndPayloadAsync().Forget();
    }

    private async UniTask TeleportDroneAndPayloadAsync()
    {
        await UniTask.Yield();

        bool hasReleasedPayload = _dronePayloadReleaseSystem.HasReleasedPayload;
        RigidbodyInterpolation cachedInterpolation = RigidbodyInterpolation.None;
        if (hasReleasedPayload == false)
        {
            cachedInterpolation = _payloadRigidbody.interpolation;
            _payloadRigidbody.interpolation = RigidbodyInterpolation.None;
            _payloadRigidbody.isKinematic = false;
        }

        _droneRigidbody.position = _spawnPositionTransform.position;
        _droneRigidbody.rotation = Quaternion.identity;
        _droneRigidbody.velocity = Vector3.zero;
        _droneRigidbody.angularVelocity = Vector3.zero;

        if (hasReleasedPayload == false)
        {
            _payloadRigidbody.interpolation = cachedInterpolation;
            _payloadRigidbody.isKinematic = true;
        }
    }

    private void OnDestroy() 
    {
        UnsubscribeToTeleportButtonPerformed();
        UnsubscribeToDronePayloadCreated();
    }

    private void UnsubscribeToTeleportButtonPerformed()
    {
        _inputActions.Drone.TeleportToSpawnPosition.performed -= HandleTeleportButtonPerformed;
    }

    public void Init(Transform spawnPositionTransform)
    { 
        _spawnPositionTransform = spawnPositionTransform;
    }

    private void Awake()
    {
        if (_dronePayloadReleaseSystem != null)
        {
            SubscribeToDronePayloadCreated();
        }
    }

    private void SubscribeToDronePayloadCreated()
    {
        _dronePayloadReleaseSystem.OnPayloadCreated += GetRigidbodyFromCreatedPayload;
    }

    private void GetRigidbodyFromCreatedPayload(Rigidbody createdPayload)
    {
        _payloadRigidbody = createdPayload;
    }

    private void UnsubscribeToDronePayloadCreated()
    {
        _dronePayloadReleaseSystem.OnPayloadCreated -= GetRigidbodyFromCreatedPayload;
    }
}