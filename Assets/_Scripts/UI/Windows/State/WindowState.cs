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

    public void Close()
    {
        _baseWindow.gameObject.SetActive(false);
        HandleClose();
    }

    public virtual void HandleClose() { }

    public virtual bool ShouldClosePreviousWindow()
    {
        return true;
    }
}
