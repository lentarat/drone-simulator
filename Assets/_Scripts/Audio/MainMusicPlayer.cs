using Zenject;
using UnityEngine;

public class MainMusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip _mainMusicAudioClip;
    [SerializeField] private float _volume;
    
    private AudioController _audioController;

    [Inject]
    public void Construct (AudioController audioController)
    {
        _audioController = audioController;
    }

    private void Start()
    {
        _audioController.Play(_mainMusicAudioClip, _volume, audioType: AudioController.AudioType.Music);
    }
}
