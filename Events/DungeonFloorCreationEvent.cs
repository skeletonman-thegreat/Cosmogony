public class DungeonFloorCreationEvent
{
    public int TargetLevel { get; }

    public DungeonFloorCreationEvent(int targetLevel)
    {
        TargetLevel = targetLevel;
    }
}
