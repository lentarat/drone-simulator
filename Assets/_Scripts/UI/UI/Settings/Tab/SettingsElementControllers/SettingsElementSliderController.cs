using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

public class SettingsElementSliderController : SettingsElementController
{
    [SerializeField] private Slider _slider;

    private int _minValue;
    private int _maxValue;

    public void Init(int currentValue, int minValue, int maxValue)
    {
        _minValue = minValue;
        _maxValue = maxValue;

        base.Init(currentValue);

        SetSliderRange(minValue, maxValue);
        UpdateSlider(currentValue);

        SubscribeToSliderValueChange();
    }

    private void SetSliderRange(int minValue, int maxValue)
    { 
        _slider.minValue = minValue;
        _slider.maxValue = maxValue;
    }

    private void UpdateSlider(int newValue)
    {
        _slider.value = newValue;
    }

    protected override int GetAdjustedCurrentValue(int currentValue)
    {
        int value = Mathf.Clamp(currentValue, _minValue, _maxValue);
        return value;
    }

    protected override void HandleArrowButtonClicked(int offset)
    {
        base.HandleArrowButtonClicked(offset);
        UpdateSlider(CurrentValue);
    }

    private void SubscribeToSliderValueChange()
    {
        _slider.onValueChanged.AddListener(ChangeCurrentValue);
    }

    private void ChangeCurrentValue(float newValue)
    {   
        SetCurrentValue((int)newValue);
    }
}
