using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameModeWindow : BackableWindow
{
    [Header("GameModeWindow fields")]
    [SerializeField] private Button _freeFlyingModeButton;
    [SerializeField] private Button _groundTargetsModeButton;
    [SerializeField] private Button _airborneTargetsModeButton;
    [SerializeField] private GameSettingsSO _gameSettingsSO;

    protected override void Awake()
    {
        base.Awake();

        SubscribeToButtons();
    }

    private void SubscribeToButtons()
    {
        _freeFlyingModeButton.onClick.AddListener(HandleFreeFlyingModeButtonClicked);
        _groundTargetsModeButton.onClick.AddListener(HandleGroundTargetsModeButtonClicked);
        _airborneTargetsModeButton.onClick.AddListener(HandleAirborneModeButtonClicked);
    }

    private void HandleFreeFlyingModeButtonClicked()
    {
        _gameSettingsSO.GameModeType = GameModeType.FreeFlying;
        SceneManager.LoadScene(1);
    }

    private void HandleGroundTargetsModeButtonClicked()
    {
        _gameSettingsSO.GameModeType = GameModeType.GroundTargets;
        SceneManager.LoadScene(1);
    }

    private void HandleAirborneModeButtonClicked()
    {
        _gameSettingsSO.GameModeType = GameModeType.AirborneTargets;
        SceneManager.LoadScene(1);
    }
}
