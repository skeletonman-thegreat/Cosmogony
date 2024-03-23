public class PlayerInitializationSystem
{
    private readonly ComponentManager componentManager;
    private readonly int playerEntityId;

    public PlayerInitializationSystem(ComponentManager componentManager, int playerEntityId)
    {
        this.componentManager = componentManager;
        this.playerEntityId = playerEntityId;
        EventDispatcher.Subscribe<LevelLoadedEvent>(e => OnLevelLoaded((LevelLoadedEvent)e));
    }

    private void OnLevelLoaded(LevelLoadedEvent e)
    {
        Point spawnPoint = e.SpawnPoint;
        componentManager.UpdateComponent(playerEntityId, new PositionComponent { X = spawnPoint.X, Y = spawnPoint.Y, IsValid = true });
        EventDispatcher.Emit(new MovementCompletedEvent(playerEntityId, spawnPoint.X, spawnPoint.Y));
        // Trigger visibility update or any other initial setup required for the player
        // This could involve emitting another event or directly invoking the necessary systems/methods
    }
}
