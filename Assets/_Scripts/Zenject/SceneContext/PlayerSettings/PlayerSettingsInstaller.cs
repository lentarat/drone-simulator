using UnityEngine;
using Zenject;

public class PlayerSettingsInstaller : MonoInstaller
{
    [SerializeField] private PlayerSettingsSO _playerSettingsSO;

    public override void InstallBindings()
    {
        Container.Install<PlayerSettingsSignalBusInstaller>();
        Debug.Log("Installed");
        Container.Install<PlayerSettingsHandlersInstaller>();

        Container.BindInterfacesAndSelfTo<PlayerSettingsChangesInformer>().AsSingle().NonLazy();
        Container.Bind<PlayerSettingsSO>().FromScriptableObject(_playerSettingsSO).AsSingle();
        Container.Bind<IPlayerSettingsStorageProvider>().To<PlayerSettingsStorageJSON>().AsSingle();
    }
}