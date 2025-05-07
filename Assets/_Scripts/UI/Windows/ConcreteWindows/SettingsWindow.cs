using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SettingsWindow : BackableWindow
{
    [Header("SettingsWindow fields")]
    [SerializeField] private ButtonToSettingsTab[] _buttonToSettingsTabs;
    [SerializeField] private SettingsTab _openedTab;
    [SerializeField] private Button _applyChangesButton;
    [SerializeField] private PlayerSettingsSO _playerSettingsSO;

    private SignalBus _signalBus;
    private IPlayerSettingsStorageProvider _playerSettingsStorageProvider;

    [Inject]
    private void Construct(SignalBus signalBus)
    { 
        _signalBus = signalBus;
    }

    protected override void Awake()
    {
        base.Awake();

        LoadPlayerSettings();
        ManageEachSettingsTab();
        SubcribeToAnySettingsValuesChanged();
        SubscribeToApplyChangesButtonClick();
    }

    private void LoadPlayerSettings()
    {
        _playerSettingsStorageProvider = new PlayerSettingsStorageJSON();
        _playerSettingsStorageProvider.LoadTo(_playerSettingsSO);
    }

    private void ManageEachSettingsTab()
    {
        foreach (ButtonToSettingsTab buttonToSettingsTab in _buttonToSettingsTabs)
        {
            SubscribeToTabHeaderButton(buttonToSettingsTab);
            InitializeSettingsTab(buttonToSettingsTab.SettingsTab);
        }
    }

    private void SubscribeToTabHeaderButton(ButtonToSettingsTab buttonToSettingsTab)
    {
        SettingsTab tab = buttonToSettingsTab.SettingsTab;
        buttonToSettingsTab.HeaderButton.onClick.AddListener(() => ChangeOpenedTab(tab));
    }

    private void InitializeSettingsTab(SettingsTab settingsTab)
    {
        settingsTab.Init(_playerSettingsSO);
    }

    private void ChangeOpenedTab(SettingsTab tab)
    {
        _openedTab.Hide();
        tab.Show();
        _openedTab = tab;
    }

    private void SubcribeToAnySettingsValuesChanged()
    {
        SettingsElementController.OnValueChanged += ShowApplyChangesButton;
    }

    private void ShowApplyChangesButton()
    {
        _applyChangesButton.gameObject.SetActive(true);
    }

    private void SubscribeToApplyChangesButtonClick()
    {
        _applyChangesButton.onClick.AddListener(ApplyChanges);
    }

    private void ApplyChanges()
    {
        SaveOpenedSettingsTab();
        InformPlayerSettingsSettingChanged();
        SavePlayerSettingsIntoStorage();
        HideApplyChangesButton();
    }

    private void SaveOpenedSettingsTab()
    {
        _openedTab.SaveConcretePlayerSettings();
    }

    private void InformPlayerSettingsSettingChanged()
    {
        _signalBus.Fire(new PlayerSettingsChangedSignal(_playerSettingsSO));
    }

    private void SavePlayerSettingsIntoStorage()
    {
        _playerSettingsStorageProvider.Save(_playerSettingsSO);
    }

    private void HideApplyChangesButton()
    {
        _applyChangesButton.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        UnsubcribeToAnySettingsValuesChanged();
        UnsubscribeToApplyChangesButtonClick();
    }

    private void UnsubcribeToAnySettingsValuesChanged()
    {
        SettingsElementController.OnValueChanged -= ShowApplyChangesButton;
    }

    private void UnsubscribeToApplyChangesButtonClick()
    {
        _applyChangesButton.onClick.RemoveListener(ApplyChanges);
    }
}
