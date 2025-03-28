using Cysharp.Threading.Tasks;
public abstract class WindowState
{
    private bool _isClosing;
    private bool _isOpening;
    private BaseWindow _baseWindow;
    protected BaseWindow BaseWindow => _baseWindow;

    public WindowState(BaseWindow baseWindow)
    { 
        _baseWindow = baseWindow;
    }

    public async void Open()
    {
        if (_isOpening || _isClosing)
            return;

        _isOpening = true;

        _baseWindow.gameObject.SetActive(true);
        await HandleOpen();

        _isOpening = false;
    }

    public virtual UniTask HandleOpen()
    {
        return UniTask.CompletedTask;
    }

    public async void Close()
    {
        if(_isClosing || _isOpening) 
            return;
        
        _isClosing = true;
        
        await HandleClose();
        
        _isClosing = false;
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
