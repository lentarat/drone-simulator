using UnityEngine;
using Zenject;

public class TargetSpawnerInstaller : MonoInstaller
{
    [Header("Targets")]
    [SerializeField] private GroundTarget _groundTargetPrefab;
    [SerializeField] private AirborneTarget _airborneTargetPrefab;
    [Header("ScriptableObjects")]
    [SerializeField] private GameSettingsSO _gameSettingsSO;
    [SerializeField] private DifficultyLevelTargetSettingsSO _difficultyLevelTargetSettingsSO;
    [SerializeField] private DifficultyLevelTargetSpawnerSettingsSO _difficultyLevelTargetSpawnerSettingsSO;
    [Header("Other")]
    [SerializeField] private Transform _targetsParent;
    [SerializeField] private GameObject[] _routesParents;

    public override void InstallBindings()
    {
        BindTargetFactoryAndTargetSpawner();
        BindTargetsParent();
        BindRoutesParent();
        BindTargets();
        BindScriptableObjects();
    }

    private void BindTargetFactoryAndTargetSpawner()
    {
        Container.Bind<TargetFactory<GroundTarget>>().To<GroundTargetFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<TargetSpawner<GroundTarget>>().AsSingle().NonLazy();
    }

    private void BindTargetsParent()
    {
        Container.Bind<Transform>().WithId("TargetsParent").FromInstance(_targetsParent);
    }

    private void BindRoutesParent()
    {
        Container.Bind<GameObject[]>().WithId("RoutesParents").FromInstance(_routesParents);
    }

    private void BindTargets()
    {
        Container.Bind<GroundTarget>().FromInstance(_groundTargetPrefab);
        Container.Bind<AirborneTarget>().FromInstance(_airborneTargetPrefab);
    }

    private void BindScriptableObjects()
    {
        Container.Bind<GameSettingsSO>().FromScriptableObject(_gameSettingsSO).AsSingle();
        Container.Bind<DifficultyLevelTargetSettingsSO>().FromScriptableObject(_difficultyLevelTargetSettingsSO).AsSingle();
        Container.Bind<DifficultyLevelTargetSpawnerSettingsSO>().FromScriptableObject(_difficultyLevelTargetSpawnerSettingsSO).AsSingle();
    }
}
