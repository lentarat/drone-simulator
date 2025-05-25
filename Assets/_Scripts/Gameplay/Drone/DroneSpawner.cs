using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DroneSpawner : MonoBehaviour
{
    [SerializeField] private DroneHUD _droneHUD;

    private DroneFactory _droneFactory;

    [Inject]
    private void Construct(DroneFactory droneFactory)
    { 
        _droneFactory = droneFactory;
    }

    private void Awake()
    {
        Drone spawnedDrone = SpawnDrone();
        InitDroneSpawnPositionTeleporter(spawnedDrone);
        InitDroneHUD(spawnedDrone);
    }

    private Drone SpawnDrone()
    {
        Drone drone = _droneFactory.Create();
        return drone;
    }

    private void InitDroneSpawnPositionTeleporter(Drone drone)
    {
        DroneSpawnPositionTeleporter droneSpawnPositionTeleporter = drone.DroneSpawnPositionTeleporter;
        droneSpawnPositionTeleporter.Init(transform);
    }

    private void InitDroneHUD(Drone drone)
    {
        _droneHUD.Init(drone.DroneMovementSystem, drone.DroneAltimeter);
    }
}
