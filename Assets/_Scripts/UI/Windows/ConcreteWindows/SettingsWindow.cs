using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : BackableWindow
{
    [Header("SettingsWindow fields")]
    [SerializeField] private ButtonToSettingsTab[] _buttonToSettingsTabs;
    [SerializeField] private SettingsTab _openedTab;
    [SerializeField] private Button _applyChangesButton;
    [SerializeField] private PlayerSettingsSO _playerSettingsSO;

    private PlayerSettingsStorage _playerSettingsStorage;

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
        _playerSettingsStorage = new PlayerSettingsStorage();
        _playerSettingsStorage.LoadTo(_playerSettingsSO);
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
        settingsTab.Initialize(_playerSettingsSO);
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
        SaveEachSettingsTab();
        _playerSettingsStorage.Save(_playerSettingsSO);
        HideApplyChangesButton();
    }

    private void SaveEachSettingsTab()
    {
        foreach (ButtonToSettingsTab buttonToSettingsTab in _buttonToSettingsTabs)
        {
            buttonToSettingsTab.SettingsTab.SaveConcretePlayerSettings();
        }
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
