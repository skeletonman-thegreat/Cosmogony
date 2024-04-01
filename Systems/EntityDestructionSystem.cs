public class EntityDestructionSystem
{
    private ComponentManager componentManager;
    private EntityManager entityManager;

    public EntityDestructionSystem(ComponentManager componentManager, EntityManager entityManager)
    {
        this.componentManager = componentManager;
        this.entityManager = entityManager;
        EventDispatcher.Subscribe<EntityDestructionEvent>(HandleEntityDestructionEvent);
    }

    public void HandleEntityDestructionEvent(EntityDestructionEvent e)
    {
        componentManager.RemoveAllComponents(e.EntityID);
        entityManager.DestroyEntity(e.EntityID);
    }
}
