using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WindowState
{
    private BaseWindow _baseWindow;
    protected BaseWindow BaseWindow => _baseWindow;

    public WindowState(BaseWindow baseWindow)
    { 
        _baseWindow = baseWindow;
    }

    public virtual void Open()
    {
        _baseWindow.gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        _baseWindow.gameObject.SetActive(false);
    }

    public virtual bool ShouldClosePreviousWindow()
    {
        return true;
    }
}
