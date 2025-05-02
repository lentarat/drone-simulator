using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettingsStorage
{
    private string _path = "player_settings.json";

    public void Save(PlayerSettingsSO playerSettingsSO)
    {
        JsonDataService.SaveData<PlayerSettingsSO>(playerSettingsSO, _path);
    }

    public void LoadTo(PlayerSettingsSO playerSettingsSO)
    {
        JsonDataService.LoadDataTo(playerSettingsSO, _path);
    }
}
