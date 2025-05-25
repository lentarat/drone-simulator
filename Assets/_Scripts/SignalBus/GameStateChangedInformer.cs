using System;
using Zenject;

public class GameStateChangedInformer
{
    private readonly SignalBus _signalBus;

    public GameStateChangedInformer(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    public void InformGameStateChanged(GameState newGameState)
    {
        _signalBus.Fire(new GameStateChangedSignal(newGameState));
    }
}
