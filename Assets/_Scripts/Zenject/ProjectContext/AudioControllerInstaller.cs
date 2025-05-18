using UnityEngine;
using Zenject;

public class AudioControllerInstaller : MonoInstaller
{
    [SerializeField] private AudioController _audioController;

    public override void InstallBindings()
    {
        Container.Bind<AudioController>().FromComponentInNewPrefab(_audioController).AsSingle();
    }
}
