public class GroundTargetFactory : TargetFactory<GroundTarget>
{
    public GroundTargetFactory(
        GameSettingsSO gameSettingsSO,
        DifficultyLevelTargetSettingsSO difficultyLevelTargetSettingsSO,
        GroundTarget groundTargetPrefab) : base(gameSettingsSO, difficultyLevelTargetSettingsSO)
    {
        TargetPrefab = groundTargetPrefab;
    }
}
