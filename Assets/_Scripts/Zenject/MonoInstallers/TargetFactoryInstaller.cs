using Zenject;

public class TargetFactoryInstaller : MonoInstaller, IInitializable
{
    public override void InstallBindings()
    {
        //Container.BindInterfacesTo<TargetFactoryInstaller>();
        //Container.BindInterfacesAndSelfTo<TargetFa>();
    }

    void IInitializable.Initialize()
    {
        throw new System.NotImplementedException();
    }
}