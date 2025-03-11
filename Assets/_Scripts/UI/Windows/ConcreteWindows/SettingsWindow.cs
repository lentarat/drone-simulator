using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : BackableWindow
{
    protected override void SetWindowState()
    {
        WindowState = new OverlappedWindowState(this);
    }
}
