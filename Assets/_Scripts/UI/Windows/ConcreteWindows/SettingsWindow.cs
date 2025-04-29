using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsWindow : BackableWindow
{
    [Header("SettingsWindow fields")]
    [SerializeField] private SettingsTab[] _settingsTabs;
    [SerializeField] private SettingsTab _openedTab;

    protected override void Awake()
    {
        base.Awake();

        SubscribeToTabs();
    }

    private void SubscribeToTabs()
    { 
        foreach(SettingsTab tab in _settingsTabs) 
        {
            tab.OnTabStateChanged += ChangeOpenedTab;
        }
    }

    private void ChangeOpenedTab(SettingsTab tab)
    {
        _openedTab.Hide();
        tab.Show();
        _openedTab = tab;
    }

    private void OnDestroy()
    {
        UnsubscribeToTabs();
    }

    private void UnsubscribeToTabs()
    {
        foreach (SettingsGeneralTab tab in _settingsTabs)
        {
            tab.OnTabStateChanged -= ChangeOpenedTab;
        }
    }
}
