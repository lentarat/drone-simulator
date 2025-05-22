using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using Zenject;

public class LanguageChanger : IInitializable, IDisposable
{
    private PlayerSettingsSO.LanguageType _currentLanguageType;

    private readonly SignalBus _signalBus;
    private readonly Dictionary<PlayerSettingsSO.LanguageType, string> _languagesToCodes = new()
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
        _signalBus.Subscribe<PlayerSettingsChangedSignal>(HandlePlayerSettingsChangedSignal);
    }

    private void HandlePlayerSettingsChangedSignal(PlayerSettingsChangedSignal signal)
    {
        PlayerSettingsSO.LanguageType newLanguageType = signal.PlayerSettingsSO.Language;
        
        bool hasLanguageChanged = HasLanguageChanged(_currentLanguageType, newLanguageType);
        if (hasLanguageChanged == false)
        {
            return;
        }

        _currentLanguageType = newLanguageType;
        string localeCode = _languagesToCodes[newLanguageType];
        SetLocale(localeCode);
    }

    private bool HasLanguageChanged(PlayerSettingsSO.LanguageType currentLanguage, PlayerSettingsSO.LanguageType newLanguage)
    {
        if (currentLanguage != newLanguage)
        {
            return true;
        }

        return false;
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
        _signalBus.Unsubscribe<PlayerSettingsChangedSignal>(HandlePlayerSettingsChangedSignal);
    }
}
