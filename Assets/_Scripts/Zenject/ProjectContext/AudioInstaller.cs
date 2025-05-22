using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class AudioInstaller : MonoInstaller
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioController _audioController;

    public override void InstallBindings()
    {
        Container.Bind<AudioController>().FromComponentInNewPrefab(_audioController).AsSingle();
        Container.Bind<AudioMixer>().FromInstance(_audioMixer).AsSingle();
        Container.Bind<AudioVolumeChanger>().AsSingle().NonLazy();
    }
}
