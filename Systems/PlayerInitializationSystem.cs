public class PlayerInitializationSystem
{
    private readonly ComponentManager componentManager;
    private readonly int playerEntityId;

    public PlayerInitializationSystem(ComponentManager componentManager, int playerEntityId)
    {
        this.componentManager = componentManager;
        this.playerEntityId = playerEntityId;
        EventDispatcher.Subscribe<LevelLoadedEvent>(OnLevelLoaded);
    }

    private void OnLevelLoaded(LevelLoadedEvent e)
    {
        Point spawnPoint = e.SpawnPoint;
        componentManager.UpdateComponent(playerEntityId, new PositionComponent { X = spawnPoint.X, Y = spawnPoint.Y, IsValid = true });
        // Trigger visibility update or any other initial setup required for the player
        EventDispatcher.Emit(new MovementCompletedEvent(playerEntityId, componentManager.GetComponent<PositionComponent>(playerEntityId).X, componentManager.GetComponent<PositionComponent>(playerEntityId).Y));
    }
}
