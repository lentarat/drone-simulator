using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DroneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<InputActions>().AsSingle();
        Container.Bind<IDroneMoveable>().To<DroneMovementInputActionsReader>().AsSingle();
        Container.Bind<IDroneBomber>().To<DroneBomberInputActionsReader>().AsSingle();
    }
}
