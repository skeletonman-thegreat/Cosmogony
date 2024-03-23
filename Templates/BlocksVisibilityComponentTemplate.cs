public class BlocksVisibilityComponentTemplate : ComponentTemplate
{
    public override void AddComponentTo(int entityId, ComponentManager componentManager)
    {
        componentManager.AddComponent(entityId, new BlocksVisibilityComponent());
    }
}