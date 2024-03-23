public class LevelTransitionSystem
{
    private WorldSystem worldSystem;

    public LevelTransitionSystem(WorldSystem worldSystem)
    {
        this.worldSystem = worldSystem;
        EventDispatcher.Subscribe<LevelTransitionEvent>(e => HandleLevelTransition((LevelTransitionEvent)e));
    }

    private void HandleLevelTransition(LevelTransitionEvent e)
    {
        var currentLevelIndex = worldSystem.CurrentLevelIndex;
        int targetLevel;

        switch (e.Direction)
        {
            case TransitionDirection.Ascend:
                targetLevel = Math.Max(0, currentLevelIndex - 1); // Prevent going below 0
                break;
            case TransitionDirection.Descend:
                targetLevel = currentLevelIndex + 1; // Assuming there's no upper limit
                break;
            default:
                throw new InvalidOperationException("Unknown transition direction.");
        }

        if (!worldSystem.IsValidLevelIndex(targetLevel))
        {
            // If the target level doesn't exist, create it.
            EventDispatcher.Emit(new DungeonFloorCreationEvent(targetLevel));
        }
        else
        {
            // For existing levels, just get the map.
            DungeonLevel existingLevel = worldSystem.GetLevelByIndex(targetLevel);
            // Ensure entities for the transitioned level are initialized with the existing map.
            EventDispatcher.Emit(new EntityInitializationEvent(targetLevel, existingLevel.Map));
        }

        // Update the current level index after handling.
        worldSystem.CurrentLevelIndex = targetLevel;
    }

}
