using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class AudioInstaller : MonoInstaller
{
    [SerializeField] private AudioController _audioController;
    [SerializeField] private MainMusicPlayer _mainMusicPlayer;
    [SerializeField] private AudioMixer _audioMixer;

    public override void InstallBindings()
    {
        Container.Bind<AudioController>().FromComponentInNewPrefab(_audioController).AsSingle();
        Container.Bind<MainMusicPlayer>().FromComponentInNewPrefab(_mainMusicPlayer).AsSingle().NonLazy();
        Container.Bind<AudioMixer>().FromInstance(_audioMixer).AsSingle();
        Container.Bind<AudioVolumeChanger>().AsSingle().NonLazy();
    }
}
