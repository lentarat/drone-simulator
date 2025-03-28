using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameModeWindow : BackableWindow
{
    [Header("GameModeWindow field")]
    [SerializeField] private Button _playButton;

    protected override void Awake()
    {
        base.Awake();

        SubscribeToButton();
    }

    private void SubscribeToButton()
    {
        _playButton.onClick.AddListener(() => { SceneManager.LoadScene(1); });
    }
}
