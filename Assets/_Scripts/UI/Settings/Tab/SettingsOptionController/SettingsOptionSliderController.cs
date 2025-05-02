using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsOptionSliderController : SettingsOptionController
{
    [SerializeField] private Slider _slider;

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
