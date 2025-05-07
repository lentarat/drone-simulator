public class PlayerSettingsStorageJSON : IPlayerSettingsStorageProvider
{
    private string _path = "player_settings.json";

    void IPlayerSettingsStorageProvider.Save(PlayerSettingsSO playerSettingsSO) 
    {
        JsonDataService.SaveData(playerSettingsSO, _path);
    }

    void IPlayerSettingsStorageProvider.LoadTo(PlayerSettingsSO playerSettingsSO)
    {
        JsonDataService.LoadDataTo(playerSettingsSO, _path);
    }
}
