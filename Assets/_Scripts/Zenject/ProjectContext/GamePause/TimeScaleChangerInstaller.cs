using Zenject;

public class TimeScaleChangerInstaller : Installer
{
    public override void InstallBindings()
    {
        Container.Bind<TimeScaleChanger>().AsSingle().NonLazy();
    }
}
