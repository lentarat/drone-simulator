using System;
using UnityEngine.InputSystem;

public class GameMenuInputActionsReader
{
    public event Action OnOpenWindowButtonClicked;

    private InputActions _inputActions;

    public GameMenuInputActionsReader(InputActions inputActions)
    {
        _inputActions = inputActions;
        _inputActions.UI.GameMenu.performed += HandleGameMenuOpenButtonClicked;
    }

    private void HandleGameMenuOpenButtonClicked(InputAction.CallbackContext context)
    {
        OnOpenWindowButtonClicked?.Invoke();
    }

    ~GameMenuInputActionsReader()
    {
        _inputActions.UI.GameMenu.performed -= HandleGameMenuOpenButtonClicked;
    }
}
