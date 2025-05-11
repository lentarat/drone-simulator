using Zenject;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenuWindow : BaseWindow
{
    [SerializeField] private Button _resumeGameButton;
    [SerializeField] private Button _restartGameButton;
    [SerializeField] private Button _exitToMainMenuButton;
    
    private GameMenuInputActionsReader _inputActionsReader;
    private ISceneLoader _sceneLoader;

    [Inject]
    private void Construct(GameMenuInputActionsReader inputActionsReader, ISceneLoader sceneLoader)
    {
        _inputActionsReader = inputActionsReader;
        _sceneLoader = sceneLoader;

        SubscribeToOpenWindowEvent();
    }

    private void SubscribeToOpenWindowEvent()
    {
        _inputActionsReader.OnOpenWindowButtonClicked += HandleOpenWindowButtonClicked;
    }

    private void HandleOpenWindowButtonClicked()
    {
        if (IsWindowActive<SettingsWindow>())
        {
            return;
        }
        else
        { 
            gameObject.SetActive(!gameObject.activeInHierarchy);
        }
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
        _sceneLoader.LoadScene(SceneType.MainMenu);
    }

    private void OnDestroy()
    {
        UnsubscribeToOpenWindowEvent();
    }

    private void UnsubscribeToOpenWindowEvent()
    {
        _inputActionsReader.OnOpenWindowButtonClicked -= HandleOpenWindowButtonClicked;
    }
}
