public class PositionSystem
{
    private ComponentManager componentManager;

    public PositionSystem(ComponentManager componentManager)
    {
        this.componentManager = componentManager;
    }

    public int? GetEntityAtPosition(int x, int y)
    {
        foreach (var entityId in componentManager.GetAllEntitiesWithComponent<PositionComponent>())
        {
            var position = componentManager.GetComponent<PositionComponent>(entityId);
            if (position.X == x && position.Y == y && position.IsValid)
            {
                return entityId; // Found the entity at the given position
            }
        }
        return null; // No entity found at the given position
    }
}
