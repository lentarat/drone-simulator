using UnityEngine;
using Zenject;

public class SFXPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private float _volume = 1f;
    [SerializeField] private float _pitch = 1f;

    private AudioController _audioController;

    [Inject]
    private void Construct(AudioController audioController)
    {
        _audioController = audioController;
    }

    public void Init(AudioController audioController)
    { 
        _audioController = audioController;
    }

    public void Play()
    {
        _audioController.Play(_audioClip, _volume, _pitch);
    }

    public void Play(float volume = 1f, float pitch = 1f)
    {
        _audioController.Play(_audioClip, volume, pitch);
    }
}
