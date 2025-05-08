using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DroneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IDroneMoveable>().To<DroneMovementInputActionsReader>().AsSingle();
        Container.Bind<IPayloadReleaseInvoker>().To<DroneAttackInputActionsReader>().AsSingle();
    }
}
