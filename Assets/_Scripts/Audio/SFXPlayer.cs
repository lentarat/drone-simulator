using UnityEngine;
using Zenject;

public class SFXPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private float _volume = 1f;
    [SerializeField] private float _pitch = 1f;

    private AudioController _audioController;

    //Uncomment in case compile-time gameobjects with this component are present in a scene.
    //[Inject]
    //private void Construct(AudioController audioController)
    //{ 
    //    _audioController = audioController;
    //}

    public void Init(AudioController audioController)
    { 
        _audioController = audioController;
    }

    public void Play()
    {
        _audioController.Play(_audioClip, _volume, _pitch);
    }
}
