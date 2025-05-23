using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class AudioVolumeChanger
{
    private int _currentPlayerSettingsSoundVolume = -1;
    private int _currentPlayerSettingsMusicVolume = -1;
    private float _maxMusicVolume = 10f;
    private float _lastMaxMusicVolumeSubtractionValue;
    private SceneType _lastSceneType = SceneType.MainMenu;

    private const float _minMusicVolume = -40f;
    private const float _minSoundVolume = -40f;
    private const float _maxSoundVolume = 10f;
    private const float _muteVolume = -80f;
    private const float _musicVolumeInGameSubtractionMultiplier = 0.2f;
    private const float _maxPlayerSettingsAudioVolume = 100f;
    private const float _muteNormalizedVolumeThreshold = 0.05f;
    private const string _soundVolume = "SoundVolume";
    private const string _musicVolume = "MusicVolume";
    private readonly AudioMixer _audioMixer;
    private readonly SignalBus _signalBus;
    private readonly ISceneLoader _sceneLoader;

    public AudioVolumeChanger(SignalBus signalBus, AudioMixer audioMixer, ISceneLoader sceneLoader)
    {
        _audioMixer = audioMixer;
        _signalBus = signalBus;
        _sceneLoader = sceneLoader;

        SubscribePlayerSettingsChangedSignal();
        SubscribeToGameStateChangedSignal();
        SubscribeToSceneChange();
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
            soundVolumeDB = Mathf.Lerp(_minSoundVolume, _maxSoundVolume, soundVolumeNormalized);
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
            musicVolumeDB = Mathf.Lerp(_minMusicVolume, _maxMusicVolume, musicVolumeNormalized);
        }
        _audioMixer.SetFloat(_musicVolume, musicVolumeDB);
    }

    private void SubscribeToGameStateChangedSignal()
    {
        _signalBus.Subscribe<GameStateChangedSignal>(HandleGameStateChanged);
    }

    private void HandleGameStateChanged(GameStateChangedSignal signal)
    {
        GameState newGameState = signal.NewGameState;

        if (newGameState == GameState.Normal)
        {
            UnpauseAllAudio();
        }
        else
        {
            PauseAllAudio();
        }
    }

    private void PauseAllAudio()
    {
        AudioListener.pause = true;
    }

    private void UnpauseAllAudio()
    {
        AudioListener.pause = false;
    }

    private void SubscribeToSceneChange()
    {
        _sceneLoader.OnSceneChanged += HandleSceneChanged;
    }

    private void HandleSceneChanged(SceneType sceneType)
    {
        if (sceneType == SceneType.MainMenu)
        {
            UnpauseAllAudio();
            MultiplyMusicVolumeBy(1 / _musicVolumeInGameSubtractionMultiplier);
        }
        else if(_lastSceneType == SceneType.MainMenu)
        { 
            MultiplyMusicVolumeBy(_musicVolumeInGameSubtractionMultiplier);
        }
    }

    private void MultiplyMusicVolumeBy(float volumeMultiplier)
    {
        if (volumeMultiplier < 1)
        {
            float musicVolumeRange;
            if (_maxMusicVolume > 0)
            {
                musicVolumeRange = Mathf.Abs(_minMusicVolume) + _maxMusicVolume;
            }
            else
            {
                musicVolumeRange = Mathf.Abs(_minMusicVolume) - Mathf.Abs(_maxMusicVolume);
            }

            float maxMusicVolumeChangeValue = musicVolumeRange * volumeMultiplier;
            _maxMusicVolume -= maxMusicVolumeChangeValue;
            _lastMaxMusicVolumeSubtractionValue = maxMusicVolumeChangeValue;
        }
        else
        {
            _maxMusicVolume += _lastMaxMusicVolumeSubtractionValue;
        }

        ChangeMusicVolume(_currentPlayerSettingsMusicVolume);
    }

    ~AudioVolumeChanger()
    {
        UnsubscribePlayerSettingsChangedSignal();
        UnsubscribeToGameStateChangedSignal();
        UnsubscribeToSceneChange();
    }

    private void UnsubscribePlayerSettingsChangedSignal()
    {
        _signalBus.Unsubscribe<PlayerSettingsChangedSignal>(HandlePlayerSettingsChangedSignal);
    }

    private void UnsubscribeToGameStateChangedSignal()
    {
        _signalBus.Unsubscribe<GameStateChangedSignal>(HandleGameStateChanged);
    }

    private void UnsubscribeToSceneChange()
    {
        _sceneLoader.OnSceneChanged -= HandleSceneChanged;
    }
}
