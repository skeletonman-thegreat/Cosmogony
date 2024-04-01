public class EntityFactorySystem
{
    private EntityManager entityManager;
    private ComponentManager componentManager;

    public EntityFactorySystem(EntityManager entityManager, ComponentManager componentManager)
    {
        this.entityManager = entityManager;
        this.componentManager = componentManager;
        EventDispatcher.Subscribe<EntityCreationEvent>(HandleEntityCreationEvent);
    }

    private void HandleEntityCreationEvent(EntityCreationEvent e)
    {
        CreateEntity(e.ComponentTemplates);
    }

    public int CreateEntity(IEnumerable<ComponentTemplate> componentTemplates)
    {
        int entityId = entityManager.CreateEntity();
        foreach (var template in componentTemplates)
        {
            template.AddComponentTo(entityId, componentManager);
        }
        return entityId;
    }

}