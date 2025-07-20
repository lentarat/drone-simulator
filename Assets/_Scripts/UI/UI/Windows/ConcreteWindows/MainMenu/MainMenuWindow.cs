using UnityEngine;
using UnityEngine.UI;

public class MainMenuWindow : BaseWindow
{
    [SerializeField] private Button _quitButton;

    protected override void Awake()
    {
        base.Awake();
        SubscribeToQuitButton();
    }

    private void SubscribeToQuitButton()
    {
        _quitButton.onClick.AddListener(QuitApplication);
    }

    private void QuitApplication()
    { 
        Application.Quit();
    }
}
