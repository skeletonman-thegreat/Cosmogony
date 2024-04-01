public class LevelTransitionSystem
{
    private WorldSystem worldSystem;
    private ComponentManager componentManager;

    public LevelTransitionSystem(WorldSystem worldSystem, ComponentManager componentManager)
    {
        this.worldSystem = worldSystem;
        this.componentManager = componentManager;
        EventDispatcher.Subscribe<LevelTransitionEvent>(HandleLevelTransition);
    }

    private void HandleLevelTransition(LevelTransitionEvent e)
    {
        var currentLevelIndex = worldSystem.CurrentLevelIndex;
        int targetLevel;

        switch (e.Direction)
        {
            case TransitionDirection.Ascend:
                targetLevel = Math.Max(0, currentLevelIndex - 1);
                break;
            case TransitionDirection.Descend:
                targetLevel = currentLevelIndex + 1;
                break;
            default:
                throw new InvalidOperationException("Unknown transition direction.");
        }

        if (componentManager.HasComponent<PlayerComponent>(e.EntityId))
        {
            //Disable Player Input
            GameConfig.Instance.playerInputEnabled = false;

            if (!worldSystem.IsValidLevelIndex(targetLevel))
            {
                if (ValidateDescent(e.EntityId))
                {
                    // Destroy current level entities before transition
                    DestroyCurrentLevelEntities();
                    // If the target level doesn't exist, create it and initialize entities.
                    DungeonLevel newLevel = worldSystem.CreateAndAddLevel(targetLevel);
                    // Update the current level index after handling.
                    worldSystem.CurrentLevelIndex = targetLevel;
                    EmitEntityInitializationEvent(newLevel);
                    EventDispatcher.Emit(new LevelLoadedEvent(worldSystem.GetCurrentLevel().StairsUp));
                }
                else
                {
                    EventDispatcher.Emit(new MessageEvent("You wish, pray, but no stairs appear!"));
                }
            }
            else
            {
                if (ValidateAscent(e.EntityId))
                {
                    // Destroy current level entities before transition
                    DestroyCurrentLevelEntities();
                    // For existing levels, initialize entities with the existing map.
                    DungeonLevel existingLevel = worldSystem.GetLevelByIndex(targetLevel);
                    // Update the current level index after handling.
                    worldSystem.CurrentLevelIndex = targetLevel;
                    EmitEntityInitializationEvent(existingLevel);
                    EventDispatcher.Emit(new LevelLoadedEvent(worldSystem.GetCurrentLevel().StairsDown));
                }
                else if (ValidateDescent(e.EntityId))
                {
                    // Destroy current level entities before transition
                    DestroyCurrentLevelEntities();
                    // For existing levels, initialize entities with the existing map.
                    DungeonLevel existingLevel = worldSystem.GetLevelByIndex(targetLevel);
                    // Update the current level index after handling.
                    worldSystem.CurrentLevelIndex = targetLevel;
                    EmitEntityInitializationEvent(existingLevel);
                    EventDispatcher.Emit(new LevelLoadedEvent(worldSystem.GetCurrentLevel().StairsUp));
                }
                else
                {
                    EventDispatcher.Emit(new MessageEvent("You wish, pray, but no stairs appear!"));
                }
            }
            //Restore Control to the player
            GameConfig.Instance.playerInputEnabled = true;
        }
    }

    private void DestroyCurrentLevelEntities()
    {
        var entitiesToDestroy = componentManager.GetAllEntitiesWithComponent<WorldLocationComponent>()
            .Where(entity => !componentManager.HasComponent<PlayerComponent>(entity) && componentManager.GetComponent<WorldLocationComponent>(entity).FloorNumber == worldSystem.CurrentLevelIndex);
        foreach (var entityID in entitiesToDestroy)
        {
            EventDispatcher.Emit(new EntityDestructionEvent(entityID));
        }
    }

    private void EmitEntityInitializationEvent(DungeonLevel level)
    {
        // Assuming EntityInitializationEvent accepts a DungeonLevel
        EventDispatcher.Emit(new EntityInitializationEvent(level.LevelIndex, level.Map));
    }

    private bool ValidateDescent(int entityID)
    {
        if (componentManager.GetComponent<PositionComponent>(entityID).X == worldSystem.GetCurrentLevel().StairsDown.X
    && componentManager.GetComponent<PositionComponent>(entityID).Y == worldSystem.GetCurrentLevel().StairsDown.Y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool ValidateAscent(int entityID)
    {
        if (componentManager.GetComponent<PositionComponent>(entityID).X == worldSystem.GetCurrentLevel().StairsUp.X
    && componentManager.GetComponent<PositionComponent>(entityID).Y == worldSystem.GetCurrentLevel().StairsUp.Y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
