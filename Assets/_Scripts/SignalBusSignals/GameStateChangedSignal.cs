public class GameStateChangedSignal
{
    public GameState CurrentGameState { get; private set; }

    public GameStateChangedSignal(GameState newGameState)
    {
        CurrentGameState = newGameState;
    }
}
