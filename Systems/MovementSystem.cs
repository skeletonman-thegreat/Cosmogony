public class MovementSystem
{
    private readonly ComponentManager componentManager;
    private readonly PositionSystem positionSystem;

    public MovementSystem(ComponentManager componentManager, PositionSystem positionSystem)
    {
        this.componentManager = componentManager;
        this.positionSystem = positionSystem;
        EventDispatcher.Subscribe<MovementIntentEvent>(e => HandleMovementIntent((MovementIntentEvent)e));
    }

    private void HandleMovementIntent(MovementIntentEvent e)
    {
        var position = componentManager.GetComponent<PositionComponent>(e.EntityID);
        if (!position.IsValid) return; // Entity does not have a position component.

        // Check if the movement is valid.
        if (IsValidMove(position.X + e.Dx, position.Y + e.Dy))
        {
            // Apply movement.
            position.X += e.Dx;
            position.Y += e.Dy;
            componentManager.UpdateComponent(e.EntityID, position); // Update position component.
            EventDispatcher.Emit(new MovementCompletedEvent(e.EntityID, position.X, position.Y));
        }

        // Optionally, reset or handle the movement intent further.
    }


private bool IsValidMove(int newX, int newY)
    {
        // Get the entity at the target position, if any
        var entityId = positionSystem.GetEntityAtPosition(newX, newY);

        // If no entity is at the target position, the move is valid
        if (!entityId.HasValue) return true;

        // Check if the entity has a CollisionComponent
        if (componentManager.HasComponent<CollisionComponent>(entityId.Value))
        {
            var collisionComponent = componentManager.GetComponent<CollisionComponent>(entityId.Value);

            // If the entity is marked as having collision, movement is not valid
            if (collisionComponent.HasCollision) return false;
        }

        // If there's an entity but it doesn't have collision or is passable, movement is valid
        return true;
    }

}
