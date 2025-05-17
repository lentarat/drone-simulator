using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using Zenject;

public class LanguageChanger : IInitializable, IDisposable
{
    private SignalBus _signalBus;
    private Dictionary<PlayerSettingsSO.LanguageType, string> _languagesToCodes = new()
    {
        { PlayerSettingsSO.LanguageType.English, "en" },
        { PlayerSettingsSO.LanguageType.Ukrainian, "uk" }
    };

    public LanguageChanger(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    void IInitializable.Initialize()
    {
        _signalBus.Subscribe<PlayerSettingsChangedSignal>(HandlePlayerSettingsChanged);
    }

    private void HandlePlayerSettingsChanged(PlayerSettingsChangedSignal signal)
    {
        PlayerSettingsSO.LanguageType languageType = signal.PlayerSettingsSO.Language;
        string localeCode = _languagesToCodes[languageType];
        SetLocale(localeCode);
    }

    private void SetLocale(string localeCode)
    {
        Locale selectedLocale = LocalizationSettings.AvailableLocales.Locales
            .FirstOrDefault(locale => locale.Identifier.Code == localeCode);

        if (selectedLocale == null)
        {
            Debug.LogError($"Language with code: {localeCode} not found.");
        }
        else
        {
            LocalizationSettings.SelectedLocale = selectedLocale;
        }
    }

    void IDisposable.Dispose()
    {
        _signalBus.Unsubscribe<PlayerSettingsChangedSignal>(HandlePlayerSettingsChanged);
    }
}
