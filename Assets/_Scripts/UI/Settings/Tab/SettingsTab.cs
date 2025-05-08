using UnityEngine;
using TMPro;

public abstract class SettingsTab : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _headerText;

    public bool WasEnabled { get; set; }
    protected PlayerSettingsSO PlayerSettingsSO { get; private set; }

    public abstract void SaveConcretePlayerSettings();
    protected abstract void ProvideCurrentValuesToControllers();

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
        ProvideCurrentValuesToControllers();
    }

    private void OnEnable()
    {
        WasEnabled = true;
    }
}
