
using System;
using UnityEngine;

[Serializable]
public struct EnumToSettingsOptionController<TEnum> where TEnum : Enum
{
    [SerializeField] private SettingsElementOptionController _controller;
    public SettingsElementOptionController Controller => _controller;
}

