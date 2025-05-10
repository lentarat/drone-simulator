using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

public class SettingsElementSliderController : SettingsElementController
{
    [SerializeField] private Slider _slider;

    private int _minValue;
    private int _maxValue;

    public void Init(StringTable localizationTable, int currentValue, int minValue, int maxValue)
    {
        base.Init(localizationTable, currentValue);
        _minValue = minValue; 
        _maxValue = maxValue;
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

    private void UpdateSlider(int newValue)
    { 
        _slider.value = newValue;
    }
}
