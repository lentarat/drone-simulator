using Zenject;

public class PlayerSettingsListenersInstaller : Installer
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<LanguageChanger>().AsSingle();
    }
}