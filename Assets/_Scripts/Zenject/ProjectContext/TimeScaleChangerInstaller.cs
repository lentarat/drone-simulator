using Zenject;

public class TimeScaleChangerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<TimeScaleChanger>().AsSingle().NonLazy();
    }
}
