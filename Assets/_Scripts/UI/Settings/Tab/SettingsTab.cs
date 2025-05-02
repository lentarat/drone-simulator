using UnityEngine;
using TMPro;
using System;

public abstract class SettingsTab : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _headerText;

    protected PlayerSettingsSO PlayerSettingsSO { get; private set; }

    public abstract void SaveConcretePlayerSettings();

    public void Initialize(PlayerSettingsSO playerSettingsSO)
    {
        PlayerSettingsSO = playerSettingsSO;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        ChangeFontToSelected();
    }

    private void ChangeFontToSelected()
    {
        _headerText.fontStyle = FontStyles.Bold | FontStyles.Underline;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        ChangeFontToUnselected();
    }

    private void ChangeFontToUnselected()
    {
        _headerText.fontStyle = FontStyles.Normal;
    }

    protected TEnum GetAdjustedEnumValue<TEnum>(SettingsOptionController settingsOptionController) where TEnum : Enum
    {
        int value = settingsOptionController.CurrentValue;
        int length = Enum.GetValues(typeof(TEnum)).Length;
        int adjustedIndex = (value % length + length) % length;
        return (TEnum)Enum.GetValues(typeof(TEnum)).GetValue(adjustedIndex);
    }

    protected int GetAdjustedIntValue(int value, int minValue, int maxValue)
    { 
        value = Math.Clamp(value, minValue, maxValue);
        return value;
    }
}
