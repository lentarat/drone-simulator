using Zenject;

public class GamePauserInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GamePauser>().AsSingle();
    }
}
