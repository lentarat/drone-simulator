using UnityEngine;
using Zenject;

public class PlayerSettingsInstaller : MonoInstaller
{
    [SerializeField] private PlayerSettingsSO _playerSettingsSO;

    public override void InstallBindings()
    {
        Container.Install<PlayerSettingsHandlersInstaller>();

        Container.BindInterfacesAndSelfTo<PlayerSettingsChangesInformer>().AsSingle().NonLazy();

        Container.Bind<IPlayerSettingsStorageProvider>().To<PlayerSettingsStorageJSON>().AsSingle();
        Container.Bind<PlayerSettingsSO>().FromScriptableObject(_playerSettingsSO).AsSingle();
    }
}