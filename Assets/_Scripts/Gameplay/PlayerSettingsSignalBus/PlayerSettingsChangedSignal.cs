using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettingsChangedSignal 
{
    public PlayerSettingsSO PlayerSettingsSO { get; private set; }

    public PlayerSettingsChangedSignal(PlayerSettingsSO playerSettingsSO)
    {
        PlayerSettingsSO = playerSettingsSO;      
    }
}
