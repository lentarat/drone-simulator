using UnityEngine;
using TMPro;
using System;

public abstract class SettingsTab : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _headerText;

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
}
