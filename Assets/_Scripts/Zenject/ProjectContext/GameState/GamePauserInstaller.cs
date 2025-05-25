using Zenject;

public class GamePauserInstaller : Installer
{
    public override void InstallBindings()
    {
        Container.Bind<GamePauser>().AsSingle();
    }
}
