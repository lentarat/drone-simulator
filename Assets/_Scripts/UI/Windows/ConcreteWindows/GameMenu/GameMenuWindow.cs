using Zenject;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenuWindow : BaseWindow
{
    [SerializeField] private Button _exitToMainMenuButton;
    
    private GameMenuInputActionsReader _inputActionsReader;

    [Inject]
    private void Construct(GameMenuInputActionsReader inputActionsReader)
    {
        _inputActionsReader = inputActionsReader;
        SubscribeToOpenWindowEvent();
    }

    private void SubscribeToOpenWindowEvent()
    {
        _inputActionsReader.OnOpenButtonClicked += ShowWindow;
    }

    private void ShowWindow()
    {
        gameObject.SetActive(true);
    }

    protected override WindowState[] GetChosenWindowStates()
    {
        WindowState[] chosenWindowState = { new NormalWindowState(this) };
        return chosenWindowState;
    }

    protected override void Awake()
    {
        base.Awake();
        SubscribeToButtons();
    }

    private void SubscribeToButtons()
    {
        _exitToMainMenuButton.onClick.AddListener(ExitToMainMenu);
    }

    private void ExitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void OnDestroy()
    {
        UnsubscribeToOpenWindowEvent();
    }

    private void UnsubscribeToOpenWindowEvent()
    {
        _inputActionsReader.OnOpenButtonClicked += ShowWindow;
    }
}
