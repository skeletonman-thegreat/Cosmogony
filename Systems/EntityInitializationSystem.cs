using System.IO;
using System.Reflection.Metadata;
using Newtonsoft.Json;

public class EntityInitializationSystem
{
    private readonly string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "JSON", "dungeon_tiles.json"); //path to JSON file for tiles
    private Dictionary<char, Tile> tileDefinitions;

    public EntityInitializationSystem()
    {
        EventDispatcher.Subscribe<EntityInitializationEvent>(HandleEntityInitializationEvent);
    }

    private void HandleEntityInitializationEvent(object eventObj)
    {
        if (eventObj is EntityInitializationEvent e)
        {
            LoadAndProcessTileDefinitions(); // Ensures tile definitions are loaded

            for (int y = 0; y < e.DungeonMap.GetLength(1); y++)
            {
                for (int x = 0; x < e.DungeonMap.GetLength(0); x++)
                {
                    char tileChar = e.DungeonMap[x, y];
                    if (tileDefinitions.ContainsKey(tileChar))
                    {
                        Tile tile = tileDefinitions[tileChar];
                        List<ComponentTemplate> templates = ConvertToComponentTemplates(tile);
                        templates.Add(new PositionComponentTemplate(x, y, true)); // Add position based on tile location
                        templates.Add(new WorldLocationComponentTemplate(GameConfig.Instance.dungeonName, e.LevelIndex));
                        EventDispatcher.Emit(new EntityCreationEvent(templates));
                    }
                }
            }
        }
    }


    private void LoadAndProcessTileDefinitions()
    {
        // Assume configPath is correctly set to the path of the JSON file
        string jsonText = File.ReadAllText(configPath);
        TileData config = JsonConvert.DeserializeObject<TileData>(jsonText);

        tileDefinitions = new Dictionary<char, Tile>();
        foreach (var tile in config.Tiles)
        {
            if (!string.IsNullOrEmpty(tile.Type) && tile.Type.Length == 1)
            {
                char tileChar = tile.Type[0];
                tileDefinitions[tileChar] = tile;
            }
        }
    }

    private List<ComponentTemplate> ConvertToComponentTemplates(Tile tile)
    {
        List<ComponentTemplate> templates = new List<ComponentTemplate>();

        // Assuming these template classes are already defined and take the necessary parameters
        if (tile.Components.RenderComponent.Initialized)
        {
            templates.Add(new RenderComponentTemplate(tile.Components.RenderComponent.Symbol, tile.Components.RenderComponent.Color));
        }
        if (tile.Components.CollisionComponent.Initialized)
        {
            templates.Add(new CollisionComponentTemplate(tile.Components.CollisionComponent.HasCollision));
        }
        if (tile.Components.ExploredComponent.Initialized)
        {
            templates.Add(new ExploredComponentTemplate(tile.Components.ExploredComponent.IsExplored));
        }
        if (tile.Components.VisibleComponent.Initialized)
        {
            templates.Add(new VisibleComponentTemplate(tile.Components.VisibleComponent.IsVisible));
        }
        if (tile.Components.BlocksVisibilityComponent.Initialized)
        {
            // Assuming BlocksVisibilityComponentTemplate has a parameterless constructor or is a struct with default values
            templates.Add(new BlocksVisibilityComponentTemplate());
        }
        if (tile.Components.TerrainComponent.Initialized)
        {
            templates.Add(new TerrainComponentTemplate());
        }
        if (tile.Components.PriorityDrawComponent.Initialized)
        {
            templates.Add(new PriorityDrawComponentTemplate());
        }

        // Add more component templates based on your JSON structure and ECS design

        return templates;
    }




}
