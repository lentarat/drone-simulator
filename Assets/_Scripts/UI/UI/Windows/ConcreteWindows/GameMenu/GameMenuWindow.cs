using Zenject;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenuWindow : BaseWindow
{
    [SerializeField] private Button _resumeGameButton;
    [SerializeField] private Button _restartGameButton;
    [SerializeField] private Button _exitToMainMenuButton;
    
    private GamePauser _gamePauser;
    private GameMenuInputActionsReader _inputActionsReader;
    private ISceneLoader _sceneLoader;

    [Inject]
    private void Construct(GameMenuInputActionsReader inputActionsReader, GamePauser gamePauser, ISceneLoader sceneLoader)
    {
        _inputActionsReader = inputActionsReader;
        _gamePauser = gamePauser;
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
            ToggleGameWindow();
        }
    }

    private void ToggleGameWindow()
    {
        ToggleWindowState();
        ToggePause();
    }

    private void ToggleWindowState()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }

    private void ToggePause()
    {
        _gamePauser.TogglePause();
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
        _resumeGameButton.onClick.AddListener(ResumeGame);
        _restartGameButton.onClick.AddListener(RestartGame);
        _exitToMainMenuButton.onClick.AddListener(ExitToMainMenu);
    }

    private void ResumeGame()
    {
        ToggleGameWindow();
    }

    private void RestartGame()
    {
        ToggePause();
        _sceneLoader.ReloadCurrentScene();
    }

    private void ExitToMainMenu()
    {
        ToggePause();
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
