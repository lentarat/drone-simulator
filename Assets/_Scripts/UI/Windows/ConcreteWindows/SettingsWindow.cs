using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : BackableWindow
{
    protected override void SetWindowState()
    {
        WindowState[] windowStates = GetChosenWindowStates();
        WindowState = new CombinedWindowState(this, windowStates);
        //WindowState = new OverlappedWindowState(this);
    }

    private WindowState[] GetChosenWindowStates()
    {
        WindowState[] chosenWindowStates = {
            new OverlappedWindowState(this),
            new FadedWindowState(this)
        };
        return chosenWindowStates;
    }
}
