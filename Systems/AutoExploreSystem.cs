public class AutoExploreSystem
{
    private ComponentManager componentManager;
    private AStarPathfinder pathfinder;
    private PositionSystem positionSystem;
    private WorldSystem worldSystem;
    private int playerEntityId;
    private bool isAutoExploring = false;
    private Queue<Point> currentPath = new Queue<Point>(); // Queue to hold the current path

    public AutoExploreSystem(ComponentManager componentManager, PositionSystem positionSystem, WorldSystem worldSystem, int playerEntityId)
    {
        this.componentManager = componentManager;
        this.positionSystem = positionSystem;
        this.worldSystem = worldSystem;
        this.playerEntityId = playerEntityId;
        this.pathfinder = new AStarPathfinder(worldSystem.GetCurrentLevel().Map);

        EventDispatcher.Subscribe<AutoExploreEvent>(OnAutoExplore);
        EventDispatcher.Subscribe<AutoExploreInterruptEvent>(OnAutoExploreInterrupt);
        EventDispatcher.Subscribe<MovementCompletedEvent>(OnMovementCompleted);
    }

    private void OnAutoExplore(AutoExploreEvent e)
    {
        if (e.EntityId == playerEntityId) // Check if the event is for the player
        {
            isAutoExploring = true;
            ProceedWithExploration();
        }
    }

    private void OnAutoExploreInterrupt(AutoExploreInterruptEvent e)
    {
        if (e.EntityId == playerEntityId)
        {
            isAutoExploring = false;
            currentPath.Clear(); // Clear the current path
        }
    }

    private void OnMovementCompleted(MovementCompletedEvent e)
    {
        if (e.EntityID == playerEntityId && isAutoExploring)
        {
            ProceedWithExploration();
        }
    }

    private void ProceedWithExploration()
    {
        if (!isAutoExploring || currentPath.Count == 0)
        {
            UpdatePath();
        }
        else
        {
            MoveToNextStep();
        }
    }

    private void UpdatePath()
    {
        var playerPositionComponent = componentManager.GetComponent<PositionComponent>(playerEntityId);
        if (playerPositionComponent.IsValid)
        {
            var goal = FindExplorationGoal(playerPositionComponent);
            if (goal.HasValue)
            {
                var path = pathfinder.FindPath(new Point(playerPositionComponent.X, playerPositionComponent.Y), goal.Value);
                if (path.Any())
                {
                    // Skip the first point since it's the player's current position
                    currentPath = new Queue<Point>(path.Skip(1));
                    MoveToNextStep();
                }
                else
                {
                    isAutoExploring = false;
                }
            }
        }
    }

    private void MoveToNextStep()
    {
        if (currentPath.Count > 0)
        {
            var nextStep = currentPath.Dequeue();
            var playerPositionComponent = componentManager.GetComponent<PositionComponent>(playerEntityId);
            EventDispatcher.Emit(new MovementIntentEvent(playerEntityId, nextStep.X - playerPositionComponent.X, nextStep.Y - playerPositionComponent.Y));
        }
    }

    private Point? FindExplorationGoal(PositionComponent currentPosition)
    {
        // Placeholder for fetching the map dimensions or relevant area
        char[,] map = worldSystem.GetCurrentLevel().Map;
        int mapWidth = map.GetLength(0);
        int mapHeight = map.GetLength(1);

        Point? bestGoal = null;
        int bestAttractiveness = int.MinValue;

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                var entityAtPosition = positionSystem.GetEntityAtPosition(x, y);
                if (entityAtPosition.HasValue && !componentManager.HasComponent<PlayerComponent>(entityAtPosition.Value))
                {
                    var goalComponent = componentManager.GetComponent<ExplorationGoalComponent>(entityAtPosition.Value);
                    var isVisibleComponent = componentManager.GetComponent<VisibleComponent>(entityAtPosition.Value);
                    if (!goalComponent.IsExplored && goalComponent.Attractiveness > bestAttractiveness)
                    {
                        bestAttractiveness = goalComponent.Attractiveness;
                        bestGoal = new Point(x, y);
                    }
                }
            }
        }

        return bestGoal;
    }

}
