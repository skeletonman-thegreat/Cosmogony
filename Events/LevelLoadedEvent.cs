public class LevelLoadedEvent
{
    public Point SpawnPoint { get; private set; }

    public LevelLoadedEvent(Point spawnPoint)
    {
        SpawnPoint = spawnPoint;
    }
}
