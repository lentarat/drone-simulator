using System;
using UnityEngine.Localization.Components;
using UnityEngine;
using UnityEditor;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization.Settings;
using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SettingsElementOptionController : SettingsElementController
{
    [SerializeField] private LocalizeStringEvent _localizedStringEvent;

    private string _localizationTableName = "UI_Settings";
    private StringTable _localizationTable;
    private Type _enumType;

    public void Init<TEnum>(int currentValue) where TEnum : Enum
    {
        _enumType = typeof(TEnum);
        base.Init(currentValue);
    }

    private void Start()
    {
        FindTableAsync().Forget();
    }

    //private void FindTable()
    //{
    //    var tableOperation = LocalizationSettings.StringDatabase.GetTableAsync(_localizationTableName);
    //    tableOperation.Completed += handle =>
    //    {
    //        _localizationTable = handle.Result;
    //        UpdateOption(CurrentValue);
    //    };
    //}

    private async UniTask FindTableAsync()
    {
        try
        {
            AsyncOperationHandle<StringTable> tableOperation = LocalizationSettings.StringDatabase.GetTableAsync(_localizationTableName);
            _localizationTable = await tableOperation.Task;

            UpdateOption();
        }
        catch (Exception ex)
        { 
            Debug.LogError($"Failed to load localization table: {ex.Message}");
        }
    }

    private void UpdateOption()
    {
        string localizedStringKey = GetUpdatedLocalizedStringKey();
        //string localizedStringKey = $"SettingsWindow.Drone.FlightMode.{value}";
        Debug.Log("Option Updated to key: " + localizedStringKey);
        _localizedStringEvent.StringReference.TableEntryReference = localizedStringKey;
    }

    private string GetUpdatedLocalizedStringKey()
    {
        string value = GetEnumCurrentValueString();
        long keyId = _localizedStringEvent.StringReference.TableEntryReference.KeyId;
        string localizedStringKey;
        if (keyId == 0)
        {
            string key = _localizedStringEvent.StringReference.TableEntryReference.Key;
            localizedStringKey = _localizationTable.GetEntry(key).Key;
        }
        else
        {
            localizedStringKey = _localizationTable.GetEntry(keyId).Key;
        }

        int lastDotIndex = localizedStringKey.LastIndexOf('.');
        if (lastDotIndex >= 0)
        {
            localizedStringKey = localizedStringKey.Substring(0, lastDotIndex + 1);
        }

        localizedStringKey += value;

        return localizedStringKey;
    }

    private string GetEnumCurrentValueString()
    {
        string value = Enum.ToObject(_enumType, CurrentValue).ToString();
        return value;
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
        UpdateOption();
    }
}
