using System;
using UnityEngine.Localization.Components;
using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization.Settings;
using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SettingsElementOptionController : SettingsElementController
{
    [SerializeField] private LocalizeStringEvent _localizedStringEvent;

    private StringTable _localizationTable;
    private Type _enumType;

    public void Init<TEnum>(StringTable localizationTable, int currentValue) where TEnum : Enum
    {
        _enumType = typeof(TEnum);
        _localizationTable = localizationTable;

        base.Init(currentValue);

        UpdateOption();
    }

    private void UpdateOption()
    {
        string localizedStringKey = GetUpdatedLocalizedStringKey();
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
