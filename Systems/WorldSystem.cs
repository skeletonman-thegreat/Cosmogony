public class WorldSystem
{
    private List<DungeonLevel> levels; // Tracks all dungeon levels
    public int CurrentLevelIndex { get; set; } = 0; // Index of the current level

    public WorldSystem()
    {
        this.levels = new List<DungeonLevel>();
        EventDispatcher.Subscribe<DungeonFloorCreationEvent>(e => HandleDungeonFloorCreation((DungeonFloorCreationEvent)e));
    }

    private void HandleDungeonFloorCreation(DungeonFloorCreationEvent e)
    {
        if (e.TargetLevel >= levels.Count)
        {
            // Generate new floor and add it to levels
            var dungeonMap = new DungeonGenerator().GenerateDungeon();
            var (stairsUp, stairsDown) = FindStairsLocations(dungeonMap);
            var level = new DungeonLevel(dungeonMap, e.TargetLevel, stairsUp, stairsDown);
            levels.Add(level);

            // Emit event to signal entity initialization for this new level
            EmitEntityInitializationEvent(e.TargetLevel);
            EmitLevelLoadedEvent(stairsUp);
        }
    }

    public DungeonLevel CreateAndAddLevel(int levelIndex)
    {
        var dungeonMap = new DungeonGenerator().GenerateDungeon();
        var (stairsUp, stairsDown) = FindStairsLocations(dungeonMap);
        var level = new DungeonLevel(dungeonMap, levelIndex, stairsUp, stairsDown);
        levels.Add(level);
        return level;
    }

    // Method to retrieve a level by its index
    public DungeonLevel GetLevelByIndex(int levelIndex)
    {
        if (IsValidLevelIndex(levelIndex))
        {
            return levels[levelIndex];
        }
        return null; // Or consider throwing an exception if the index is invalid
    }

    private void EmitEntityInitializationEvent(int levelIndex)
    {
        var level = GetLevelByIndex(levelIndex);
        if (level != null)
        {
            var dungeonMap = level.Map;
            EventDispatcher.Emit(new EntityInitializationEvent(levelIndex, dungeonMap));
        }
    }

    private void EmitLevelLoadedEvent(Point stairsUp)
    {
        if(stairsUp.X != null && stairsUp.Y != null)
        {
            EventDispatcher.Emit(new LevelLoadedEvent(stairsUp));
        }
        else
        {
            throw new Exception("Stairs Do Not Exist");
        }
    }

    public bool IsValidLevelIndex(int levelIndex)
    {
        return levelIndex >= 0 && levelIndex < levels.Count;
    }

    public DungeonLevel GetCurrentLevel()
    {
        if (CurrentLevelIndex >= 0 && CurrentLevelIndex < levels.Count)
        {
            return levels[CurrentLevelIndex];
        }
        return null;
    }

    public (Point stairsUp, Point stairsDown) FindStairsLocations(char[,] dungeonMap)
    {
        Point stairsUp = new Point(-1, -1);
        Point stairsDown = new Point(-1, -1);

        for (int x = 0; x < dungeonMap.GetLength(0); x++)
        {
            for (int y = 0; y < dungeonMap.GetLength(1); y++)
            {
                if (dungeonMap[x, y] == '<')
                {
                    stairsUp = new Point(x, y);
                }
                else if (dungeonMap[x, y] == '>')
                {
                    stairsDown = new Point(x, y);
                }
            }
        }

        return (stairsUp, stairsDown);
    }


    // Other utility methods as needed.. we'll see!
}

// Represents a dungeon level, could be expanded with more properties as needed
public class DungeonLevel
{
    public char[,] Map { get; private set; }
    public int LevelIndex { get; private set; }

    public Point StairsUp { get; private set; }

    public Point StairsDown { get; private set; }

    public DungeonLevel(char[,] map, int levelIndex, Point stairsUp, Point stairsDown)
    {
        Map = map;
        LevelIndex = levelIndex;
        StairsUp = stairsUp;
        StairsDown = stairsDown;
    }
}
