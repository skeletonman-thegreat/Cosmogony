public class TileData
{
    public List<Tile> Tiles { get; set; }
}

public class Tile
{
    public string Type { get; set; }
    public Components Components { get; set; }
}

public class Components
{
    public RenderComponent RenderComponent { get; set; }
    public CollisionComponent CollisionComponent { get; set; }
    public ExploredComponent ExploredComponent { get; set; }
    public VisibleComponent VisibleComponent { get; set; }
    public BlocksVisibilityComponent BlocksVisibilityComponent { get; set; }
    public TerrainComponent TerrainComponent { get; set; }
    public PriorityDrawComponent PriorityDrawComponent { get; set; }
    public WorldLocationComponent WorldLocationComponent { get; set; }
    public ExplorationGoalComponent ExplorationGoalComponent { get; set; }
}