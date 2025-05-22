using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class SettingsGeneralTab : SettingsTab
{
    [SerializeField] private EnumToSettingsOptionController<PlayerSettingsSO.LanguageType> _languageController;

    public override void SaveConcretePlayerSettings()
    {
        PlayerSettingsSO.Language = (PlayerSettingsSO.LanguageType)_languageController.Controller.GetEnumCurrentValue();
    }

    protected override async UniTask ProvideCurrentValuesToControllersAsync()
    {
        await base.ProvideCurrentValuesToControllersAsync();

        int languageCurrentValue = Convert.ToInt32(PlayerSettingsSO.Language);
        _languageController.Controller.Init<PlayerSettingsSO.LanguageType>(LocalizationTable, languageCurrentValue);
    }
}
