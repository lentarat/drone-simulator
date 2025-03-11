using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public abstract class BaseWindow : MonoBehaviour
{
    [SerializeField] private ButtonClickToWindow[] _buttonClicksToWindowStates;

    private WindowState _windowState;
    public WindowState WindowState
    {
        get => _windowState;
        protected set => _windowState = value;
    }

    [Serializable]
    public struct ButtonClickToWindow
    {
        [SerializeField] private Button _button;
        public Button Button => _button;
        [SerializeField] private BaseWindow _window;
        public BaseWindow Window => _window;
    }

    protected virtual void SetWindowState()
    {
        _windowState = new NormalWindowState(this);
    }

    private void Awake()
    {
        SetWindowState();
    }

    private void Start()
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
