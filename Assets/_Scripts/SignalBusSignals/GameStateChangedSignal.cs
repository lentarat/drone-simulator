public class GameStateChangedSignal
{
    public GameState NewGameState { get; private set; }

    public GameStateChangedSignal(GameState newGameState)
    {
        NewGameState = newGameState;
    }
}
