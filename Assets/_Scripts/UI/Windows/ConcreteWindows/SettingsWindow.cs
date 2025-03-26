public class SettingsWindow : BackableWindow
{
    protected override WindowState[] GetChosenWindowStates()
    {
        WindowState[] chosenWindowStates = {
            new OverlappedWindowState(this),
            new FadedWindowState(this)
        };
        return chosenWindowStates;
    }
}
