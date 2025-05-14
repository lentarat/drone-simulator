using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSourcePrefab;
    
    private BehaviourPool<AudioSource> _audioSourcePool;

    public void Play(AudioClip audioClip, float volume = 1f, float pitch = 1f)
    {
        AudioSource audioSource = _audioSourcePool.Get();
        audioSource.clip = audioClip;
        audioSource.volume = volume;    
        audioSource.pitch = pitch;
        audioSource.Play();

        ReleaseAudioSourceAfter(audioSource, audioClip.length).Forget();
    }

    private async UniTask ReleaseAudioSourceAfter(AudioSource audioSource, float time)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(time));
        _audioSourcePool.Release(audioSource);
    }

    private void Awake()
    {
        _audioSourcePool = new(_audioSourcePrefab, parent: transform);
    }
}
