using Cysharp.Threading.Tasks;
public abstract class WindowState
{
    private BaseWindow _baseWindow;
    protected BaseWindow BaseWindow => _baseWindow;

    public WindowState(BaseWindow baseWindow)
    { 
        _baseWindow = baseWindow;
    }

    public void Open()
    {
        _baseWindow.gameObject.SetActive(true);
        HandleOpen();
    }

    public virtual void HandleOpen() { }

    public async void Close()
    {
        await HandleClose();
        _baseWindow.gameObject.SetActive(false);
    }

    public virtual UniTask HandleClose() 
    {
        return UniTask.CompletedTask;
    }

    public virtual bool ShouldClosePreviousWindow()
    {
        return true;
    }
}
