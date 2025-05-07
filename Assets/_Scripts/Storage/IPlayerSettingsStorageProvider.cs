public interface IPlayerSettingsStorageProvider
{
    void Save(PlayerSettingsSO data);
    void LoadTo(PlayerSettingsSO target);
}
