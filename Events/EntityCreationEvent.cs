public class EntityCreationEvent
{
    public IEnumerable<ComponentTemplate> ComponentTemplates { get; private set; }

    public EntityCreationEvent(IEnumerable<ComponentTemplate> componentTemplates)
    {
        ComponentTemplates = componentTemplates;
    }
}
