using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinedWindowState : WindowState
{
    private WindowState[] _windowStates;

    public CombinedWindowState(BaseWindow baseWindow, WindowState[] windowStates) : base(baseWindow)
    { 
        _windowStates = windowStates;
    }

    public override void HandleOpen()
    {
        foreach (WindowState state in _windowStates)
        {
            state.HandleOpen();
        }
    }

    public override void HandleClose()
    {
        foreach (WindowState state in _windowStates)
        {
            state.HandleClose();
        }
    }

    public override bool ShouldClosePreviousWindow()
    {
        foreach (WindowState state in _windowStates)
        {
            if (state.ShouldClosePreviousWindow() == false)
            {
                return false;
            }
        }

        return true;
    }
}
