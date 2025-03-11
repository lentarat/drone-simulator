using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlappedWindowState : WindowState
{
    public OverlappedWindowState(BaseWindow baseWindow) : base(baseWindow) 
    {

    }

    public override bool ShouldClosePreviousWindow()
    {
        return false;
    }
}
