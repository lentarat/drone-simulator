using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DroneInstaller : MonoInstaller
{
    [SerializeField] private Drone _droneWithGrenade;
    [SerializeField] private Drone _droneKamikadze;
    [SerializeField] private Transform _droneParentTransform;
    [SerializeField] private GameSettingsSO _gameSettingsSO;

    public override void InstallBindings()
    {
        BindDroneFactory();

        Container.Bind<IDroneMoveable>().To<DroneMovementInputActionsReader>().AsSingle();
        Container.Bind<IPayloadReleaseInvoker>().To<DroneAttackInputActionsReader>().AsSingle();
        Container.Bind<IDroneCameraSwitchInvoker>().To<DroneCameraSwitchInputActionsReader>().AsSingle();
    }

    private void BindDroneFactory()
    {
        Drone chosenDrone;
        if (_gameSettingsSO.GameModeType == GameModeType.GrenadeDrop)
        {
            chosenDrone = _droneWithGrenade;
        }
        else
        {
            chosenDrone = _droneKamikadze;
        }

        Container
            .BindFactory<Drone, DroneFactory>()
            .FromComponentInNewPrefab(chosenDrone)
            .UnderTransform(_droneParentTransform)
            .AsSingle();
    }
}
