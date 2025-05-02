
using System;
using UnityEngine;

[Serializable]
public struct EnumToSettingsOptionController<TEnum> where TEnum : Enum
{
    [SerializeField] private TEnum _enum;
    public TEnum Enum => _enum;
    [SerializeField] private SettingsOptionController _controller;
    public SettingsOptionController Controller => _controller;
}

