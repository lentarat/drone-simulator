using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class DifficultyLevelWindow : BackableWindow
{
    [Header("DifficultyLevelWindow fields")]
    [SerializeField] private Button _easyDifficultyButton;
    [SerializeField] private Button _mediumDifficultyModeButton;
    [SerializeField] private Button _hardDifficultyButton;
    [SerializeField] private GameSettingsSO _gameSettingsSO;

    private ISceneLoader _sceneLoader;

    [Inject]
    private void Construct(ISceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }

    protected override void Awake()
    {
        base.Awake();

        SubscribeToButtons();
    }

    private void SubscribeToButtons()
    {
        _easyDifficultyButton.onClick.AddListener(() => { HandleDifficultyClicked(DifficultyLevelType.Easy); });
        _mediumDifficultyModeButton.onClick.AddListener(() => { HandleDifficultyClicked(DifficultyLevelType.Medium); });
        _hardDifficultyButton.onClick.AddListener(() => { HandleDifficultyClicked(DifficultyLevelType.Hard); });
    }

    private void HandleDifficultyClicked(DifficultyLevelType difficultyLevelType)
    {
        _gameSettingsSO.DifficultyLevelType = difficultyLevelType;
        _sceneLoader.LoadScene(SceneType.Map1);
    }
}
