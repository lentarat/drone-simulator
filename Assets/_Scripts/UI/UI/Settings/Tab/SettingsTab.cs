using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;

public abstract class SettingsTab : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _headerText;

    private string _localizationTableName = "UI_Settings";

    public bool WasEnabled { get; set; }
    protected PlayerSettingsSO PlayerSettingsSO { get; private set; }
    protected StringTable LocalizationTable { get; private set; }

    public abstract void SaveConcretePlayerSettings();

    protected virtual async UniTask ProvideCurrentValuesToControllers()
    {
        await FindTableAsync();
    }

    private async UniTask FindTableAsync()
    {
        try
        {
            AsyncOperationHandle<StringTable> tableOperation = LocalizationSettings.StringDatabase.GetTableAsync(_localizationTableName);
            LocalizationTable = await tableOperation.Task;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load localization table: {ex.Message}");
        }
    }

    public void Init(PlayerSettingsSO playerSettingsSO)
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

    private void Start()
    {
        ProvideCurrentValuesToControllers().Forget();
    }

    private void OnEnable()
    {
        WasEnabled = true;
    }
}
