public class PriorityDrawComponentTemplate : ComponentTemplate
{
    public override void AddComponentTo(int entityId, ComponentManager componentManager)
    {
        componentManager.AddComponent(entityId, new PriorityDrawComponent());
    }
}
