using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SettingsWindow : BackableWindow
{
    [Header("SettingsWindow fields")]
    [SerializeField] private ButtonToSettingsTab[] _buttonToSettingsTabs;
    [SerializeField] private SettingsTab _openedTab;
    [SerializeField] private Button _applyChangesButton;

    private bool _hasUnsavedChanges;
    private ApplyChangesButtonHighlighter _applyChangesButtonHighlighter;
    private PlayerSettingsSO _playerSettingsSO;
    private PlayerSettingsChangesInformer _playerSettingsChangesInformer;
    private IPlayerSettingsStorageProvider _playerSettingsStorageProvider;

    [Inject]
    private void Construct(
        PlayerSettingsSO playerSettingsSO,
        PlayerSettingsChangesInformer playerSettingsChangesInformer,
        IPlayerSettingsStorageProvider playerSettingsStorageProvider)
    {
        _playerSettingsSO = playerSettingsSO;
        _playerSettingsChangesInformer = playerSettingsChangesInformer;
        _playerSettingsStorageProvider = playerSettingsStorageProvider;
    }

    protected override void Awake()
    {
        base.Awake();

        Init();
        ManageEachSettingsTab();
        SubcribeToAnySettingsValuesChanged();
        SubscribeToApplyChangesButtonClick();
    }
    
    private void Init()
    {
        _applyChangesButtonHighlighter = new ApplyChangesButtonHighlighter(_applyChangesButton);
    }

    private void ManageEachSettingsTab()
    {
        foreach (ButtonToSettingsTab buttonToSettingsTab in _buttonToSettingsTabs)
        {
            SubscribeToSettingsTabHeaderButton(buttonToSettingsTab);
            InitializeSettingsTab(buttonToSettingsTab.SettingsTab);
        }
    }

    private void SubscribeToSettingsTabHeaderButton(ButtonToSettingsTab buttonToSettingsTab)
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
        _hasUnsavedChanges = true;
    }

    private void SubscribeToApplyChangesButtonClick()
    {
        _applyChangesButton.onClick.AddListener(ApplyChanges);
    }

    private void ApplyChanges()
    {
        SaveSettingsTabsEnabledBefore();
        InformPlayerSettingsChanged();
        SavePlayerSettingsIntoStorage();
        HideApplyChangesButton();

        _hasUnsavedChanges = false;
    }

    private void SaveSettingsTabsEnabledBefore()
    {
        foreach (ButtonToSettingsTab buttonToSettingsTab in _buttonToSettingsTabs)
        {
            SettingsTab tab = buttonToSettingsTab.SettingsTab;

            if (tab.WasEnabled)
            {
                tab.SaveConcretePlayerSettings();
                tab.WasEnabled = false;
            }
        }

        _openedTab.WasEnabled = true;
    }

    private void InformPlayerSettingsChanged()
    {
        _playerSettingsChangesInformer.InformPlayerSettingsChanged();
    }

    private void SavePlayerSettingsIntoStorage()
    {
        _playerSettingsStorageProvider.Save(_playerSettingsSO);
    }

    private void HideApplyChangesButton()
    {
        _applyChangesButton.gameObject.SetActive(false);
    }

    protected override bool CanCloseWindow()
    {
        if (_hasUnsavedChanges)
        {
            HighlightApplyChangesButton();
            return false;
        }

        return true;
    }

    private void HighlightApplyChangesButton()
    {
        _applyChangesButtonHighlighter.HighlightButton();
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
