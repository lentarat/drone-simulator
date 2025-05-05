using System;
using UnityEngine.Localization.Components;
using UnityEngine;
using UnityEditor;

public class SettingsElementOptionController : SettingsElementController
{
    [SerializeField] private LocalizeStringEvent _localizedStringEvent;

    private Type _enumType;

    public void Init<TEnum>(int currentValue) where TEnum : Enum
    {
        _enumType = typeof(TEnum);
        base.Init(currentValue);
        UpdateOption(CurrentValue);
    }

    public int GetEnumCurrentValue()
    { 
        object enumObject =  Enum.ToObject(_enumType, CurrentValue);
        int enumValue = Convert.ToInt32(enumObject);
        return enumValue; 
    }

    //public TEnum GetAdjustedEnumValue<TEnum>() where TEnum : Enum
    //{
    //    int value = CurrentValue;
    //    int length = Enum.GetValues(typeof(TEnum)).Length;
    //    int adjustedIndex = (value % length + length) % length;
    //    return (TEnum)Enum.GetValues(typeof(TEnum)).GetValue(adjustedIndex);
    //}

    protected override int GetAdjustedCurrentValue(int currentValue)
    {
        int length = Enum.GetValues(_enumType).Length;
        int adjustedValue = (currentValue % length + length) % length;
        return adjustedValue;
    }

    protected override void HandleArrowButtonClicked(int offset)
    {
        base.HandleArrowButtonClicked(offset);
        UpdateOption(CurrentValue);
    }

    private void UpdateOption(int newValue)
    {
        string value = GetEnumCurrentValueString();
        string localizedStringKey = $"SettingsWindow.Drone.FlightMode.{value}";
        Debug.Log("Option Updated to key: " + localizedStringKey);
        _localizedStringEvent.StringReference.TableEntryReference  = localizedStringKey;
    }

    private string GetEnumCurrentValueString()
    {
        string value = Enum.ToObject(_enumType, CurrentValue).ToString();
        return value;
    }
}
