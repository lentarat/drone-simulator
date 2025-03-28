using UnityEngine;
using UnityEngine.UI;
using System;

public abstract class BaseWindow : MonoBehaviour
{
    [Header("BaseWindow fields")]
    [SerializeField] private ButtonClickToWindow[] _buttonClicksToWindowStates;

    private WindowState _windowState;
    public WindowState WindowState => _windowState;

    [Serializable]
    public struct ButtonClickToWindow
    {
        [SerializeField] private Button _button;
        public Button Button => _button;
        [SerializeField] private BaseWindow _window;
        public BaseWindow Window => _window;
    }

    public virtual void Initialize()
    {
        SetWindowState();
    }

    private void SetWindowState()
    {
        WindowState[] windowStates = GetChosenWindowStates();
        _windowState = new CombinedWindowState(this, windowStates);
    }

    protected virtual WindowState[] GetChosenWindowStates()
    {
        WindowState[] chosenWindowState = { new FadedWindowState(this) };
        return chosenWindowState;
    }

    protected virtual void Awake()
    {
        SubscribeToButtons();
    }

    private void SubscribeToButtons()
    {
        foreach(ButtonClickToWindow buttonToWindow in _buttonClicksToWindowStates) 
        {
            WindowState windowState = buttonToWindow.Window.WindowState;
            buttonToWindow.Button.onClick.AddListener(() => OpenWindow(windowState));
        }
    }

    private void OpenWindow(WindowState newWindowState)
    {
        newWindowState.Open();

        if (newWindowState.ShouldClosePreviousWindow())
        {
            _windowState.Close();
        }
    }
}
