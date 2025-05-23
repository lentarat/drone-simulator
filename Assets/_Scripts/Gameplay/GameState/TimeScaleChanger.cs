using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TimeScaleChanger
{
    private SignalBus _signalBus;

    public TimeScaleChanger(SignalBus signalBus)
    {
        _signalBus = signalBus;
        SubscribeToGameStateChangedSignal();
    }

    private void SubscribeToGameStateChangedSignal()
    {
        _signalBus.Subscribe<GameStateChangedSignal>(HandleGameStateChanged);
    }

    private void HandleGameStateChanged(GameStateChangedSignal signal)
    {
        GameState newGameState = signal.NewGameState;

        if (newGameState == GameState.Normal)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }
    }

    ~TimeScaleChanger()
    {
        UnsubscribeToGameStateChangedSignal();
    }

    private void UnsubscribeToGameStateChangedSignal()
    {
        _signalBus.Unsubscribe<GameStateChangedSignal>(HandleGameStateChanged);
    }
}

