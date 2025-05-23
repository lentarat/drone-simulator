using Zenject;

public class GamePauseInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Install<TimeScaleChangerInstaller>();

        Container.Bind<GamePauser>().AsSingle();
    }
}
