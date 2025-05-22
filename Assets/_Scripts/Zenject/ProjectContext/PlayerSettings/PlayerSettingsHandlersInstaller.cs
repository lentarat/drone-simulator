using Zenject;

public class PlayerSettingsHandlersInstaller : Installer
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<LanguageChanger>().AsSingle();
        Container.Bind<DronePlayerSettingsChangesHandler>().AsSingle();
    }
}