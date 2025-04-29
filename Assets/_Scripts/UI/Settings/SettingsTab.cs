using UnityEngine;
using System;
using UnityEngine.EventSystems;
using TMPro;

public abstract class SettingsTab : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI _headerText;
    public event Action<SettingsTab> OnTabStateChanged;
    public void Show()
    {
        gameObject.SetActive(true);
        ChangeFontToSelected();
    }

    private void ChangeFontToSelected()
    {
        _headerText.fontStyle = FontStyles.Bold | FontStyles.Italic;
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

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        OnTabStateChanged?.Invoke(this);
    }
}
