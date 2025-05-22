using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueTextUpdater : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _sliderValueText;

    private void Awake()
    {
        _slider.onValueChanged.AddListener(UpdateText);
    }

    private void UpdateText(float newValue)
    {
        _sliderValueText.text = newValue.ToString();
    }
}
