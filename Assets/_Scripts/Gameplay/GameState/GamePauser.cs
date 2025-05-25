using UnityEngine;
using Zenject;

public class GamePauser
{
    private GameState _currentGameState;
    private GameStateChangedInformer _gameStateChangedInformer;

    public GamePauser(GameStateChangedInformer gameStateChangedInformer)
    {
        _gameStateChangedInformer = gameStateChangedInformer;
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

        _gameStateChangedInformer.InformGameStateChanged(_currentGameState);
    }
}
