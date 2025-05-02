using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public struct ButtonToSettingsTab
{
    [SerializeField] private Button _headerButton;
    public Button HeaderButton => _headerButton;
    [SerializeField] private SettingsTab _settingsTab;
    public SettingsTab SettingsTab => _settingsTab;
}
