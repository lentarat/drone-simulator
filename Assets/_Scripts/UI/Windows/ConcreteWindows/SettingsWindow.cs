using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsWindow : BackableWindow
{
    [Header("SettingsWindow fields")]
    [SerializeField] private ButtonToSettingsTab[] _buttonToSettingsTabs;
    [SerializeField] private SettingsTab _openedTab;

    protected override void Awake()
    {
        base.Awake();

        SubscribeToTabsHeaderButtons();
    }

    private void SubscribeToTabsHeaderButtons()
    {
        foreach (ButtonToSettingsTab buttonToSettingsTab in _buttonToSettingsTabs)
        {
            buttonToSettingsTab.HeaderButton.onClick.AddListener(() => ChangeOpenedTab(buttonToSettingsTab.SettingsTab));
        }
    }

    private void ChangeOpenedTab(SettingsTab tab)
    {
        _openedTab.Hide();
        tab.Show();
        _openedTab = tab;
    }
}
