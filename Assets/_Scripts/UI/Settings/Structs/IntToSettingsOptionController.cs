using System;
using UnityEngine;

[Serializable]
public struct IntToSettingsOptionController
{
    [SerializeField] private int _minValue;
    public int MinValue => _minValue;
    [SerializeField] private int _maxValue;
    public int MaxValue => _maxValue;
    [SerializeField] private SettingsElementSliderController _controller;
    public SettingsElementSliderController Controller => _controller;
}
