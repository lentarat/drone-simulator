using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using System.Linq;

public abstract class BaseWindow : MonoBehaviour
{
    [Header("BaseWindow fields")]
    [SerializeField] private ButtonClickToWindow[] _buttonClicksToWindowStates;

    private WindowState _windowState;
    public WindowState WindowState => _windowState;

    [Serializable]
    public class ButtonClickToWindow
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
        if (CanCloseWindow() == false)
            return;

        newWindowState.Open();

        if (newWindowState.ShouldClosePreviousWindow())
        {
            _windowState.Close();
        }
    }

    protected virtual bool CanCloseWindow() => true;

    protected bool IsWindowActive<T>() where T : BaseWindow
    {
        ButtonClickToWindow buttonClickToWindow =
            _buttonClicksToWindowStates.FirstOrDefault(
                buttonClickToWindowState => buttonClickToWindowState.Window.GetType() == typeof(T));

        bool isWindowActive = buttonClickToWindow != null && buttonClickToWindow.Window.gameObject.activeInHierarchy;
        return isWindowActive;
    }
}
