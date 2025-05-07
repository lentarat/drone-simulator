using Zenject;

public class PlayerSettingsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Install<PlayerSettingsSignalBusInstaller>();
        Container.Install<PlayerSettingsListenersInstaller>();
    }
}