using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameStateInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Install<TimeScaleChangerInstaller>();
        Container.Install<GamePauserInstaller>();

        Container.Bind<GameStateChangedInformer>().AsSingle().NonLazy();
    }
}
