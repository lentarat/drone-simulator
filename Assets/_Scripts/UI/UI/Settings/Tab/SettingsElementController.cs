using System;
using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

public abstract class SettingsElementController : MonoBehaviour
{
    [SerializeField] private Button _leftArrowButton;
    [SerializeField] private Button _rightArrowButton;

    private int _currentValue;
    public int CurrentValue 
    { 
        get => _currentValue;
        protected set
        { 
            int adjustedValue = GetAdjustedCurrentValue(value);
            _currentValue = adjustedValue;
        }
    }

    public static event Action OnValueChanged;

    public void Init(int currentValue)
    {
        CurrentValue = currentValue;
    }

    protected abstract int GetAdjustedCurrentValue(int currentValue);

    protected virtual void HandleArrowButtonClicked(int offset)
    {
        OffsetCurrentValue(offset);
        OnValueChanged?.Invoke();
    }

    protected virtual void OffsetCurrentValue(int offset)
    {
        CurrentValue += offset;
    }
    
    protected void SetCurrentValue(int value)
    {
        CurrentValue = value;
        OnValueChanged?.Invoke();
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
