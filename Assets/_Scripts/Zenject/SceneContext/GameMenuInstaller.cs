using Zenject;

public class GameMenuInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameMenuInputActionsReader>().AsSingle();
    }
}
