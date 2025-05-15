using UnityEngine;
using UnityEngine.UI;

public abstract class BackableWindow : BaseWindow
{
    [Header("BackableWindow fields")]
    [SerializeField] private Button _openPreviousWindowButton;  
    [SerializeField] private BaseWindow _previousWindow;

    protected override void Awake()
    {
        base.Awake();

        SubscribeToBackButton();
    }

    private void SubscribeToBackButton()
    {
        _openPreviousWindowButton.onClick.AddListener(OpenPreviousWindow);
    }

    private void OpenPreviousWindow()
    {
        if (CanCloseWindow() == false)
            return;

        _previousWindow.WindowState.Open();
        WindowState.Close();
    }
}
