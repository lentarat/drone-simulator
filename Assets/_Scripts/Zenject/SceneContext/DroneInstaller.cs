using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DroneInstaller : MonoInstaller
{
    [SerializeField] private Drone _droneWithGrenade;
    [SerializeField] private Drone _droneKamikadze;
    [SerializeField] private Transform _droneParentTransform;

    public override void InstallBindings()
    {
        Container
            .BindFactory<Drone, DroneFactory>()
            .FromComponentInNewPrefab(_droneWithGrenade)
            .UnderTransform(_droneParentTransform)
            .AsSingle();

        Container.Bind<IDroneMoveable>().To<DroneMovementInputActionsReader>().AsSingle();
        Container.Bind<IPayloadReleaseInvoker>().To<DroneAttackInputActionsReader>().AsSingle();
        Container.Bind<IDroneCameraSwitchInvoker>().To<DroneCameraSwitchInputActionsReader>().AsSingle();
    }
}
