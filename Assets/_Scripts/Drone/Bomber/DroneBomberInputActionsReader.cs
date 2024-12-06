using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DroneBomberInputActionsReader : IDroneBomber
{
    private InputActions _inputActions;
    private event Action _onBombReleaseButtonPressed;

    public DroneBomberInputActionsReader(InputActions inputActions)
    { 
        _inputActions = inputActions;
    }

    event Action IDroneBomber.OnBombReleaseCalled
    {
        add 
        {
            _onBombReleaseButtonPressed += value;
        }
        remove
        {
            _onBombReleaseButtonPressed -= value;
        }
    }
}
