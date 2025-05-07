using Zenject;

public class PlayerSettingsSignalBusInstaller : Installer
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<PlayerSettingsChangedSignal>();
    }
}
