using System;
using UnityEngine;

public class SettingsGeneralTab : SettingsTab
{
    [SerializeField] private EnumToSettingsOptionController<PlayerSettingsSO.LanguageType> _languageController;

    public override void SaveConcretePlayerSettings()
    {
        PlayerSettingsSO.Language = (PlayerSettingsSO.LanguageType)_languageController.Controller.GetEnumCurrentValue();
    }

    protected override void ProvideCurrentValuesToControllers()
    {
        int languageCurrentValue = Convert.ToInt32(PlayerSettingsSO.Language);
        _languageController.Controller.Init<PlayerSettingsSO.LanguageType>(languageCurrentValue);
    }
}
