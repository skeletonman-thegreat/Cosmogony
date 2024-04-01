public class MapUpdateSystem
{
    private WorldSystem worldSystem;

    public MapUpdateSystem(WorldSystem worldSystem)
    {
        this.worldSystem = worldSystem;
        // Add map changing subscriptions as needed
        EventDispatcher.Subscribe<EntityDestructionEvent>(OnEntityDestroyed);
        EventDispatcher.Subscribe<EntityCreationEvent>(OnEntityCreated);
        EventDispatcher.Subscribe<MovementCompletedEvent>(OnEntityMoved);
    }

    private void OnEntityMoved(MovementCompletedEvent e)
    {
        // Update the map based on entity's new and old position
    }

    private void OnEntityCreated(EntityCreationEvent e)
    {
        // Mark the entity's position on the map
    }

    private void OnEntityDestroyed(EntityDestructionEvent e)
    {
        // Clear the entity's position from the map
    }
}
