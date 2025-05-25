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
        TeleportDroneAndPayload().Forget();
    }

    private async UniTask TeleportDroneAndPayload()
    {
        bool hasReleasedPayload = false;
        if (_dronePayloadReleaseSystem != null)
        {
            hasReleasedPayload = _dronePayloadReleaseSystem.HasReleasedPayload;
        }
        RigidbodyInterpolation cachedPayloadInterpolation = RigidbodyInterpolation.None;
        if (hasReleasedPayload == false)
        {
            cachedPayloadInterpolation = _payloadRigidbody.interpolation;
            _payloadRigidbody.interpolation = RigidbodyInterpolation.None;
        }
        RigidbodyInterpolation cachedDroneInterpolation = _droneRigidbody.interpolation;
        _droneRigidbody.interpolation = RigidbodyInterpolation.None;

        _droneRigidbody.position =_spawnPositionTransform.position;
        _droneRigidbody.rotation = Quaternion.identity;
        _droneRigidbody.velocity = Vector3.zero;
        _droneRigidbody.angularVelocity = Vector3.zero;

        _droneRigidbody.interpolation = cachedDroneInterpolation;
        if (hasReleasedPayload == false)
        {
            await UniTask.WaitForFixedUpdate();

            _payloadRigidbody.interpolation = cachedPayloadInterpolation;
        }
    }

    private void OnDestroy()
    {
        UnsubscribeToTeleportButtonPerformed();
        
        if (_dronePayloadReleaseSystem != null)
        {
            UnsubscribeToDronePayloadCreated();
        }
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