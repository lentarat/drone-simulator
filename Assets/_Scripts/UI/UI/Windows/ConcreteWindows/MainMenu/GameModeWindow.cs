using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameModeWindow : BackableWindow
{
    [Header("GameModeWindow fields")]
    [SerializeField] private Button _grenadeDropModeButton;
    [SerializeField] private Button _kamikadzeModeButton;
    [SerializeField] private GameSettingsSO _gameSettingsSO;

    protected override void Awake()
    {
        base.Awake();

        SubscribeToButtons();
    }

    private void SubscribeToButtons()
    {
        _grenadeDropModeButton.onClick.AddListener(HandleGrenadeDropModeButtonClicked);
        _kamikadzeModeButton.onClick.AddListener(HandleKamikadzeModeButtonClicked);
    }

    private void HandleGrenadeDropModeButtonClicked()
    {
        _gameSettingsSO.GameModeType = GameModeType.GrenadeDrop;
    }

    private void HandleKamikadzeModeButtonClicked()
    {
        _gameSettingsSO.GameModeType = GameModeType.Kamikadze;
    }
}
