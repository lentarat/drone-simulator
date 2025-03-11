using UnityEngine;
using UnityEngine.UI;

public abstract class BackableWindow : BaseWindow
{
    [SerializeField] private Button _openPreviousWindowButton;
    [SerializeField] private BaseWindow _previousWindow;

    private void Awake()
    {
        SubscribeToBackButton();
    }

    private void SubscribeToBackButton()
    {
        _openPreviousWindowButton.onClick.AddListener(OpenPreviousWindow);
    }

    private void OpenPreviousWindow()
    {
        _previousWindow.WindowState.Open();
        WindowState.Close();
    }
}
