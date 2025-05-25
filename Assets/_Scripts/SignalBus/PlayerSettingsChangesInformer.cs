using System;
using Zenject;
using UnityEngine;

public class PlayerSettingsChangesInformer : IInitializable, IDisposable
{
    private readonly SignalBus _signalBus;
    private readonly PlayerSettingsSO _playerSettingsSO;
    private readonly IPlayerSettingsStorageProvider _playerSettingsStorageProvider;
    private readonly ISceneLoader _sceneLoader;

    public PlayerSettingsChangesInformer(
        SignalBus signalBus,
        PlayerSettingsSO playerSettingsSO,
        IPlayerSettingsStorageProvider playerSettingsStorageProvider,
        ISceneLoader sceneLoader)
    {
        _signalBus = signalBus;
        _playerSettingsSO = playerSettingsSO;
        _playerSettingsStorageProvider = playerSettingsStorageProvider;
        _sceneLoader = sceneLoader;
    }

    void IInitializable.Initialize()
    {
        _playerSettingsStorageProvider.LoadTo(_playerSettingsSO);
        InformPlayerSettingsChanged();

        _sceneLoader.OnSceneChanged += InformPlayerSettingsChanged;
    }

    public void InformPlayerSettingsChanged()
    {
        InformPlayerSettingsChanged(SceneType.None);
    }

    private void InformPlayerSettingsChanged(SceneType sceneType)
    {
        _signalBus.Fire(new PlayerSettingsChangedSignal(_playerSettingsSO));
    }

    void IDisposable.Dispose()
    {
        _sceneLoader.OnSceneChanged -= InformPlayerSettingsChanged;
    }
}
