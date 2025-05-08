using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameMenuInputActionsReader
{
    public event Action OnOpenButtonClicked;

    private InputActions _inputActions;

    public GameMenuInputActionsReader(InputActions inputActions)
    {
        _inputActions = inputActions;
        _inputActions.UI.GameMenu.performed += HandleGameMenuOpenButtonClicked;
    }

    private void HandleGameMenuOpenButtonClicked(InputAction.CallbackContext context)
    {
        OnOpenButtonClicked?.Invoke();
    }

    ~GameMenuInputActionsReader()
    {
        _inputActions.UI.GameMenu.performed -= HandleGameMenuOpenButtonClicked;
    }
}
