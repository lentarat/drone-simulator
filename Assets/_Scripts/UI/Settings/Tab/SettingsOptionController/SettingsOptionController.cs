using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class SettingsOptionController : MonoBehaviour
{
    [SerializeField] private Button _leftArrowButton;
    [SerializeField] private Button _rightArrowButton;

    public int CurrentValue { get; private set; }

    public static event Action OnValueChanged;

    protected virtual void HandleArrowButtonClicked(int offset)
    {
        ChangeCurrentValue(offset);
    }

    private void ChangeCurrentValue(int offset)
    {
        CurrentValue += offset;
        OnValueChanged?.Invoke();
    }
    
    protected void SetCurrentValue(int value)
    {
        CurrentValue = value;
    }

    private void Awake()
    {
        SubscribeToButtons();
    }

    private void SubscribeToButtons()
    {
        _leftArrowButton.onClick.AddListener(() => { HandleArrowButtonClicked(-1); });
        _rightArrowButton.onClick.AddListener(() => { HandleArrowButtonClicked(1); });
    }
}
