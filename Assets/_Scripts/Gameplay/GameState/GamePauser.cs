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
            _currentGameState = GameState.Paused;
        }
        else
        {
            _currentGameState = GameState.Normal;
        }
        
        _signalBus.Fire(new GameStateChangedSignal(_currentGameState));
    }
}
