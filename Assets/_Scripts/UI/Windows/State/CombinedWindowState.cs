using Cysharp.Threading.Tasks;

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

    public override async UniTask HandleClose()
    {
        foreach (WindowState state in _windowStates)
        {
            await state.HandleClose();
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
