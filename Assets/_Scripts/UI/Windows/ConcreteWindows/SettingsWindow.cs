using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsWindow : BackableWindow
{
    [Header("SettingsWindow fields")]
    [SerializeField] private ButtonToSettingsTab[] _buttonToSettingsTabs;
    [SerializeField] private SettingsTab _openedTab;
    [SerializeField] private Button _applyChangesButton;

    protected override void Awake()
    {
        base.Awake();

        SubscribeToTabsHeaderButtons();
        SubcribeToAnySettingsValuesChanged();
        SubscribeToApplyChangesButtonClick();
    }

    private void SubscribeToTabsHeaderButtons()
    {
        foreach (ButtonToSettingsTab buttonToSettingsTab in _buttonToSettingsTabs)
        {
            SettingsTab tab = buttonToSettingsTab.SettingsTab;
            buttonToSettingsTab.HeaderButton.onClick.AddListener(() => ChangeOpenedTab(tab));
        }
    }

    private void ChangeOpenedTab(SettingsTab tab)
    {
        _openedTab.Hide();
        tab.Show();
        _openedTab = tab;
    }

    private void SubcribeToAnySettingsValuesChanged()
    {
        SettingsOptionController.OnValueChanged += ShowApplyChangesButton;
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
        HideApplyChangesButton();
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
        SettingsOptionController.OnValueChanged -= ShowApplyChangesButton;
    }

    private void UnsubscribeToApplyChangesButtonClick()
    {
        _applyChangesButton.onClick.RemoveListener(ApplyChanges);
    }
}
