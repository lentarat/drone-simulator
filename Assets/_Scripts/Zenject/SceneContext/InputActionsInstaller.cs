using Zenject;

public class InputActionsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<InputActions>().AsSingle();
    }
}
