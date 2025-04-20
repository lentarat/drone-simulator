using UnityEngine;
using Zenject;

public class TargetSpawnerInstaller : MonoInstaller
{
    [SerializeField] private Transform _targetsParent;
    //[SerializeField] private Transform[] _targetSpawnTransforms;
    [SerializeField] private GameObject[] _routesParents;
    [SerializeField] private GameSettingsSO _gameSettingsSO;
    [SerializeField] private GroundTarget _groundTargetPrefab;
    [SerializeField] private AirborneTarget _airborneTargetPrefab;

    public override void InstallBindings()
    {
        BindTargetFactoryAndTargetSpawner();
        BindOtherDependencies();
    }

    private void BindTargetFactoryAndTargetSpawner()
    {
        GameModeType gameModeType = _gameSettingsSO.GameModeType;

        switch (gameModeType)
        {
            case GameModeType.GroundTargets:
            {
                Container.Bind<ITargetFactory<GroundTarget>>().To<GroundTargetFactory>().AsSingle();
                Container.BindInterfacesAndSelfTo<TargetSpawner<GroundTarget>>().AsSingle().NonLazy();
                break;
            }
            case GameModeType.AirborneTargets:
            {
                Container.Bind<ITargetFactory<AirborneTarget>>().To<AirborneTargetFactory>().AsSingle();
                Container.BindInterfacesAndSelfTo<TargetSpawner<AirborneTarget>>().AsSingle().NonLazy();
                break;
            }
            default:
            {   
                break;  
            }
        }
    }

    private void BindOtherDependencies()
    {
        Container.Bind<GameSettingsSO>().FromInstance(_gameSettingsSO);
        Container.Bind<Transform>().WithId("TargetsParent").FromInstance(_targetsParent);
        Container.Bind<GameObject[]>().WithId("RoutesParents").FromInstance(_routesParents);
        Container.Bind<GroundTarget>().FromInstance(_groundTargetPrefab);
        Container.Bind<AirborneTarget>().FromInstance(_airborneTargetPrefab);
    }
}
