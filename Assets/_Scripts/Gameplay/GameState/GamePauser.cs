using UnityEngine;
using Zenject;

public class GamePauser
{
    private GameState _currentGameState;
    private SignalBus _signalBus;

    public GamePauser(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    public void TogglePause()
    {
        if (_currentGameState == GameState.Normal)
        {
            Time.timeScale = 0f;
            AudioListener.pause = true;
            _currentGameState = GameState.Paused;
        }
        else
        {
            Time.timeScale = 1f;
            AudioListener.pause = false;
            _currentGameState = GameState.Normal;
        }
        
        _signalBus.Fire(new GameStateChangedSignal(_currentGameState));
    }
}
