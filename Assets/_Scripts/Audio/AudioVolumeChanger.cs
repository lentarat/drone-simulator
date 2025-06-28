using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class AudioVolumeChanger
{
    private int _currentPlayerSettingsSoundVolume = -1;
    private int _currentPlayerSettingsMusicVolume = -1;
    private PlayerSettingsSO.ToggleType _currentMusicInGameValue;
    private float _maxMusicVolume = 10f;
    private float _lastMaxMusicVolumeSubtractionValue;
    private float _musicVolumeDB;
    private SceneType _lastSceneType = SceneType.MainMenu;

    private const float _minMusicVolumeDB = -40f;
    private const float _minSoundVolumeDB = -40f;
    private const float _maxSoundVolumeDB = 10f;
    private const float _muteVolumeDB = -80f;
    private const float _musicVolumeInGameSubtractionMultiplier = 0.2f;
    private const float _maxPlayerSettingsAudioVolume = 100f;
    private const float _muteNormalizedVolumeThreshold = 0.05f;
    private const string _soundVolumeParamString = "SoundVolume";
    private const string _musicVolumeParamString = "MusicVolume";
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

        PlayerSettingsSO.ToggleType newMusicInGameValue = playerSettingsChangedSignal.PlayerSettingsSO.MusicInGame;
        bool hasMusicInGameValueChanged = HasMusicInGameValueChanged(_currentMusicInGameValue, newMusicInGameValue);
        if (hasMusicInGameValueChanged)
        {
            ChangeMusicInGameValue(newMusicInGameValue);
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
            soundVolumeDB = _muteVolumeDB;
        }
        else
        {
            soundVolumeDB = Mathf.Lerp(_minSoundVolumeDB, _maxSoundVolumeDB, soundVolumeNormalized);
        }

        ChangeVolume(_soundVolumeParamString, soundVolumeDB);
    }

    private void ChangeVolume(string volumeParamString, float volumeDB)
    {
        _audioMixer.SetFloat(volumeParamString, volumeDB);
    }

    private bool HasMusicInGameValueChanged(PlayerSettingsSO.ToggleType currentMusicInGameValue, PlayerSettingsSO.ToggleType newMusicInGameValue)
    {
        if (currentMusicInGameValue != newMusicInGameValue)
        {
            return true;
        }

        return false;
    }

    private void ChangeMusicInGameValue(PlayerSettingsSO.ToggleType newMusicInGameValue)
    {
        _currentMusicInGameValue = newMusicInGameValue;

        float newMusicVolumeDB;
        switch (newMusicInGameValue)
        {
            case PlayerSettingsSO.ToggleType.Enabled:
                {
                    newMusicVolumeDB = _musicVolumeDB;
                    break;
                }
            case PlayerSettingsSO.ToggleType.Disabled:
                {
                    bool isInMainMenu = IsInMainMenu();
                    newMusicVolumeDB = isInMainMenu ? _musicVolumeDB : _muteVolumeDB; 
                    break;
                }
            default:
                {
                    newMusicVolumeDB = _musicVolumeDB;
                    break;
                }
        }

        ChangeVolume(_musicVolumeParamString, newMusicVolumeDB);
    }

    private bool IsInMainMenu()
    {
        SceneType currentSceneType = _sceneLoader.GetCurrentSceneType();
        if (currentSceneType == SceneType.MainMenu)
        {
            return true;
        }

        return false;
    }

    private void ChangeMusicVolume(int currentMusicVolume)
    {
        _currentPlayerSettingsMusicVolume = currentMusicVolume;
        float musicVolumeNormalized = _currentPlayerSettingsMusicVolume / _maxPlayerSettingsAudioVolume;

        if (musicVolumeNormalized < _muteNormalizedVolumeThreshold)
        {
            _musicVolumeDB = _muteVolumeDB;
        }
        else
        {
            _musicVolumeDB = Mathf.Lerp(_minMusicVolumeDB, _maxMusicVolume, musicVolumeNormalized);
        }

        bool isInMainMenu = IsInMainMenu();
        if (isInMainMenu == false && _currentMusicInGameValue == PlayerSettingsSO.ToggleType.Disabled)
        {
            return;
        }
        
        ChangeVolume(_musicVolumeParamString, _musicVolumeDB);
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
            TurnOnMusicIfDisabledInGame();
        }
        else if(_lastSceneType == SceneType.MainMenu)
        { 
            MultiplyMusicVolumeBy(_musicVolumeInGameSubtractionMultiplier);
            TurnOffMusicIfDisabledInGame();
        }
    }

    private void MultiplyMusicVolumeBy(float volumeMultiplier)
    {
        if (volumeMultiplier < 1)
        {
            float musicVolumeRange;
            if (_maxMusicVolume > 0)
            {
                musicVolumeRange = Mathf.Abs(_minMusicVolumeDB) + _maxMusicVolume;
            }
            else
            {
                musicVolumeRange = Mathf.Abs(_minMusicVolumeDB) - Mathf.Abs(_maxMusicVolume);
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

    private void TurnOffMusicIfDisabledInGame()
    {
        if (_currentMusicInGameValue == PlayerSettingsSO.ToggleType.Disabled)
        {
            ChangeVolume(_musicVolumeParamString, _muteVolumeDB);
        }
    }

    private void TurnOnMusicIfDisabledInGame()
    {
        if (_currentMusicInGameValue == PlayerSettingsSO.ToggleType.Disabled)
        {
            ChangeVolume(_musicVolumeParamString, _musicVolumeDB);
        }
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
