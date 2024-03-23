public class WorldLocationComponentTemplate : ComponentTemplate
{
    private String dungeonName;
    private int floorNumber;

    public WorldLocationComponentTemplate(String dungeonName, int floorNumber)
    {
        this.dungeonName = dungeonName;
        this.floorNumber = floorNumber;
    }

    public override void AddComponentTo(int entityId, ComponentManager componentManager)
    {
        componentManager.AddComponent(entityId, new WorldLocationComponent { DungeonName = dungeonName, FloorNumber = floorNumber });
    }
}