using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuWindow : BaseWindow
{
    protected override WindowState[] GetChosenWindowStates()
    {
        WindowState[] chosenWindowState = { new NormalWindowState(this) };
        return chosenWindowState;
    }
}
