using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Drone : MonoBehaviour
{
    [SerializeField] private DroneMovementSystem _droneMovementSystem;
    [SerializeField] private DroneAltimeter _droneAltimeter;
    [SerializeField] private DroneSpawnPositionTeleporter _droneSpawnPositionTeleporter;

    private InputActions _inputActions;

    public DroneMovementSystem DroneMovementSystem => _droneMovementSystem;
    public DroneAltimeter DroneAltimeter => _droneAltimeter;
    public DroneSpawnPositionTeleporter DroneSpawnPositionTeleporter => _droneSpawnPositionTeleporter;

    [Inject]
    private void Construct(InputActions inputActions)
    { 
        _inputActions = inputActions;
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }
}
