public class TerrainComponentTemplate : ComponentTemplate
{
    public override void AddComponentTo(int entityId, ComponentManager componentManager)
    {
        componentManager.AddComponent(entityId, new TerrainComponent());
    }
}
