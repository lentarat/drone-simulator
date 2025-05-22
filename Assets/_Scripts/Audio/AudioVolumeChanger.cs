using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class AudioVolumeChanger
{
    private int _currentPlayerSettingsSoundVolume = -1;
    private int _currentPlayerSettingsMusicVolume = -1;

    private const float _minVolume = -40f;
    private const float _maxVolume = 10f;
    private const float _muteVolume = -80f;
    private const float _muteNormalizedVolumeThreshold = 0.05f;
    private const float _maxPlayerSettingsAudioVolume = 100f;
    private const string _soundVolume = "SoundVolume";
    private const string _musicVolume = "MusicVolume";
    private readonly AudioMixer _audioMixer;
    private readonly SignalBus _signalBus;

    public AudioVolumeChanger(SignalBus signalBus, AudioMixer audioMixer)
    {
        _audioMixer = audioMixer;    
        _signalBus = signalBus;

        SubscribePlayerSettingsChangedSignal();
    }

    private void SubscribePlayerSettingsChangedSignal()
    {
        _signalBus.Subscribe<PlayerSettingsChangedSignal>(HandlePlayerSettingsChangedSignal);
    }

    private void HandlePlayerSettingsChangedSignal(PlayerSettingsChangedSignal playerSettingsChangedSignal)
    {
        int newSoundVolume = playerSettingsChangedSignal.PlayerSettingsSO.Sound;
        bool hasSoundVolumeChanged = HasVolumeChanged(_currentPlayerSettingsSoundVolume, newSoundVolume);
        if (hasSoundVolumeChanged)
        { 
            ChangeSoundVolume(newSoundVolume);
        }

        int newMusicVolume = playerSettingsChangedSignal.PlayerSettingsSO.Music;
        bool hasMusicVolumeChanged = HasVolumeChanged(_currentPlayerSettingsMusicVolume, newMusicVolume);
        if (hasMusicVolumeChanged)
        {
            ChangeMusicVolume(newMusicVolume);
        }
    }

    private bool HasVolumeChanged(int currentVolume, int newVolume)
    {
        if (currentVolume != newVolume)
            return true;
        
        return false;
    }

    private void ChangeSoundVolume(int currentSoundVolume)
    {
        _currentPlayerSettingsSoundVolume = currentSoundVolume;
        float soundVolumeNormalized = _currentPlayerSettingsSoundVolume / _maxPlayerSettingsAudioVolume;
        float soundVolumeDB;
        if (soundVolumeNormalized < _muteNormalizedVolumeThreshold)
        {
            soundVolumeDB = _muteVolume;
        }
        else
        {
            soundVolumeDB = Mathf.Lerp(_minVolume, _maxVolume, soundVolumeNormalized);
        }
        _audioMixer.SetFloat(_soundVolume, soundVolumeDB);
    }

    private void ChangeMusicVolume(int currentMusicVolume)
    {
        _currentPlayerSettingsMusicVolume = currentMusicVolume;
        float musicVolumeNormalized = _currentPlayerSettingsMusicVolume / _maxPlayerSettingsAudioVolume;
        float musicVolumeDB;
        if (musicVolumeNormalized < _muteNormalizedVolumeThreshold)
        {
            musicVolumeDB = _muteVolume;
        }
        else
        {
            musicVolumeDB = Mathf.Lerp(_minVolume, _maxVolume, musicVolumeNormalized);
        }
        _audioMixer.SetFloat(_musicVolume, musicVolumeDB);
    }

    ~AudioVolumeChanger()
    {
        UnsubscribePlayerSettingsChangedSignal();
    }

    private void UnsubscribePlayerSettingsChangedSignal()
    {
        _signalBus.Unsubscribe<PlayerSettingsChangedSignal>(HandlePlayerSettingsChangedSignal);
    }
}
