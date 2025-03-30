using UnityEngine;
using Zenject;

public class TargetFactoryInstaller : MonoInstaller
{
    [SerializeField] private GameSettingsSO _gameSettingsSO;

    public override void InstallBindings()
    {
        Container.Instantiate<TargetSpawner<Target>>();
        BindTargetFactory();
        BindGameSettings();
    }

    private void BindTargetFactory()
    {
        GameModeType gameModeType = _gameSettingsSO.GameModeType;

        switch (gameModeType)
        {
            case GameModeType.GroundTargets:
            {
                Container.Bind<ITargetFactory<GroundTarget>>().To<GroundTargetFactory>().AsSingle();
                break;
            }
            case GameModeType.AirborneTargets:
            {
                Container.Bind<ITargetFactory<AirborneTarget>>().To<AirborneTargetFactory>().AsSingle();
                break;
            }
            default:
            {   
                break;
            }
        }
    }

    private void BindGameSettings()
    {
        Container.Bind<GameSettingsSO>().FromInstance(_gameSettingsSO).AsSingle();
    }
}