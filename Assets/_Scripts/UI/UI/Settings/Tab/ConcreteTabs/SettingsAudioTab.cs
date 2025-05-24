using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsAudioTab : SettingsTab
{
    [SerializeField] private IntToSettingsOptionController _musicController;
    [SerializeField] private IntToSettingsOptionController _soundController;
    [SerializeField] private EnumToSettingsOptionController<PlayerSettingsSO.ToggleType> _musicInGameToggleTypeController;
         
    public override void SaveConcretePlayerSettings()
    {
        PlayerSettingsSO.Music = _musicController.Controller.CurrentValue;
        PlayerSettingsSO.Sound = _soundController.Controller.CurrentValue;
        PlayerSettingsSO.MusicInGame = (PlayerSettingsSO.ToggleType) _musicInGameToggleTypeController.Controller.GetEnumCurrentValue();
    }

    protected override async UniTask ProvideCurrentValuesToControllersAsync()
    {
        await base.ProvideCurrentValuesToControllersAsync();

        int musicCurrentValue = PlayerSettingsSO.Music;
        _musicController.Controller.Init(musicCurrentValue, _soundController.MinValue, _soundController.MaxValue);

        int soundCurrentValue = PlayerSettingsSO.Sound;
        _soundController.Controller.Init(soundCurrentValue, _soundController.MinValue, _soundController.MaxValue);

        int musicInGameToggleTypeCurrentValue = Convert.ToInt32(PlayerSettingsSO.MusicInGame);
        _musicInGameToggleTypeController.Controller.Init<PlayerSettingsSO.ToggleType>(LocalizationTable, musicInGameToggleTypeCurrentValue);
    }
}
