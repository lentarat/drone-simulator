using System;
using Zenject;

public class PlayerSettingsChangesInformer : IInitializable, IDisposable
{
    private SignalBus _signalBus;
    private PlayerSettingsSO _playerSettingsSO;
    private IPlayerSettingsStorageProvider _playerSettingsStorageProvider;
    private ISceneLoader _sceneLoader;

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
        _signalBus.Fire(new PlayerSettingsChangedSignal(_playerSettingsSO));
    }

    void IDisposable.Dispose()
    {
        _sceneLoader.OnSceneChanged -= InformPlayerSettingsChanged;
    }
}
