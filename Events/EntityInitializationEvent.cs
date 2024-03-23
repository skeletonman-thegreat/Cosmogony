public class EntityInitializationEvent
{
    public char[,] DungeonMap { get; }
    public int LevelIndex { get; }

    public EntityInitializationEvent(int levelIndex, char[,] dungeonMap)
    {
        LevelIndex = levelIndex;
        DungeonMap = dungeonMap;
    }
}
