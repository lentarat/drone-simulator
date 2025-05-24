using Cysharp.Threading.Tasks;
using System;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSourcePrefab;
    [SerializeField] private AudioMixerGroup _soundAudioMixerGroup;
    [SerializeField] private AudioMixerGroup _musicAudioMixerGroup;

    private BehaviourPool<AudioSource> _audioSourcePool;

    public enum AudioType
    { 
        Sound,
        Music
    }

    public void Play(AudioClip audioClip, float volume = 1f, float pitch = 1f, AudioType audioType = AudioType.Sound)
    {
        AudioSource audioSource = _audioSourcePool.Get();

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.outputAudioMixerGroup = audioType switch
        {
            AudioType.Sound => _soundAudioMixerGroup,
            AudioType.Music => _musicAudioMixerGroup,
            _ => null
        };

        audioSource.Play();

        if (audioType == AudioType.Music)
        {
            audioSource.loop = true;
        }
        else
        { 
            ReleaseAudioSourceAfter(audioSource, audioClip.length).Forget();
        }
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
