using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.InputSystem;

public class DroneSpawnPositionTeleporter : MonoBehaviour
{
    [SerializeField] private Transform _spawnPositionTransform;
    [SerializeField] private Rigidbody _droneRigidbody;
    
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
        _droneRigidbody.position = _spawnPositionTransform.position;
        _droneRigidbody.velocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    private void OnDestroy() 
    {
        UnsubscribeToTeleportButtonPerformed();
    }

    private void UnsubscribeToTeleportButtonPerformed()
    {
        _inputActions.Drone.TeleportToSpawnPosition.performed -= HandleTeleportButtonPerformed;
    }
}
