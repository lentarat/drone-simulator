using Zenject;

public class GameMenuWindow : BaseWindow
{
    [Inject]
    private void Construct(GameMenuInputActionsReader inputActionsReader)
    {
        SubscribeToOpenWindowEvent(inputActionsReader);
    }

    //UNSUBSCRIBE

    private void SubscribeToOpenWindowEvent(GameMenuInputActionsReader inputActionsReader)
    {
        inputActionsReader.OnOpenButtonClicked += ShowWindow;
    }

    private void ShowWindow()
    {
        gameObject.SetActive(true);
    }

    protected override WindowState[] GetChosenWindowStates()
    {
        WindowState[] chosenWindowState = { new NormalWindowState(this) };
        return chosenWindowState;
    }
}
