using UnityEngine;
using Zenject;

public class TargetSpawnerInstaller : MonoInstaller
{
    [SerializeField] private Transform _targetScopePrefab;
    [SerializeField] private Transform[] _targetSpawnPositions;
    [SerializeField] private GameSettingsSO _gameSettingsSO;

    public override void InstallBindings()
    {
        BindTargetFactoryAndTargetSpawner();
        BindGameSettingsSO();
        BindTargetSpawnPositions();
        BindTargetScopePrefab();
    }

    private void BindTargetFactoryAndTargetSpawner()
    {
        GameModeType gameModeType = _gameSettingsSO.GameModeType;

        switch (gameModeType)
        {
            case GameModeType.GroundTargets:
            {
                Container.Bind<ITargetFactory<GroundTarget>>().To<GroundTargetFactory>().AsSingle();
                Container.Bind<TargetSpawner<GroundTarget>>().AsSingle().NonLazy();
                break;
            }
            case GameModeType.AirborneTargets:
            {
                Container.Bind<ITargetFactory<AirborneTarget>>().To<AirborneTargetFactory>().AsSingle();
                Container.Bind<TargetSpawner<AirborneTarget>>().AsSingle().NonLazy();
                break;
            }
            default:
            {   
                break;  
            }
        }
    }

    private void BindGameSettingsSO()
    {
        Container.Bind<GameSettingsSO>().FromInstance(_gameSettingsSO).AsSingle();
    }

    private void BindTargetSpawnPositions()
    {
        Container.Bind<Transform[]>().FromInstance(_targetSpawnPositions).AsSingle();
    }

    private void BindTargetScopePrefab()
    {
        Container.Bind<Transform>().FromInstance(_targetScopePrefab).AsSingle();
    }
}