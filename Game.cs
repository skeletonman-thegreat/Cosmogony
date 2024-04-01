using System.Reflection;

public class Game
{
    //ECS Strongmen
    private EntityManager entityManager;
    private ComponentManager componentManager;

    //MessageLog
    private MessageLogSystem messageLogSystem;

    //Positioning/Movement
    private PositionSystem positionSystem;
    private MovementSystem movementSystem;
    private InputHandlingSystem inputHandlingSystem;
    private AutoExploreSystem autoExploreSystem;

    //Rendering/FOV
    private FieldOfViewSystem fovSystem;
    private Renderer renderer;

    //World System & Co
    private WorldSystem worldSystem;
    private EntityInitializationSystem entityInitializationSystem;
    private EntityFactorySystem entityFactorySystem;
    private EntityDestructionSystem entityDestructionSystem;
    private LevelTransitionSystem levelTransitionSystem;
    private MapUpdateSystem mapUpdateSystem;

    //Player
    private PlayerInitializationSystem playerInitializationSystem;

    public bool isRunning { get; private set; }

    public Game()
    {
        //ECS strongmen
        entityManager = new EntityManager();
        componentManager = new ComponentManager(entityManager);

        //Message Log
        messageLogSystem = new MessageLogSystem(GameConfig.Instance.maxMessages);

        //Positioning/Movement
        positionSystem = new PositionSystem(componentManager);
        movementSystem = new MovementSystem(componentManager, positionSystem);

        //Rendering/FOV
        renderer = new Renderer(componentManager);
        fovSystem = new FieldOfViewSystem(componentManager, positionSystem, 6);

        //World System & Co
        worldSystem = new WorldSystem();
        entityInitializationSystem = new EntityInitializationSystem();
        entityFactorySystem = new EntityFactorySystem(entityManager, componentManager);
        entityDestructionSystem = new EntityDestructionSystem(componentManager, entityManager);
        levelTransitionSystem = new LevelTransitionSystem(worldSystem, componentManager);

        //Player
        int playerEntityId = entityManager.CreateEntity();
        componentManager.AddComponent(playerEntityId, new PositionComponent { X = 0, Y = 0, IsValid = true });

        playerInitializationSystem = new PlayerInitializationSystem(componentManager, playerEntityId);

        InitializeGame(playerEntityId);
        autoExploreSystem = new AutoExploreSystem(componentManager, positionSystem, worldSystem, playerEntityId);
        isRunning = true;

    }

    public void InitializeGame(int playerEntityId)
    {
        componentManager.AddComponent(playerEntityId, new PriorityDrawComponent { Initialized = true });
        componentManager.AddComponent(playerEntityId, new RenderComponent { Symbol = '@', Color = ConsoleColor.Green });
        componentManager.AddComponent(playerEntityId, new CollisionComponent { HasCollision = true });
        componentManager.AddComponent(playerEntityId, new VisibleComponent { IsVisible = true });
        componentManager.AddComponent(playerEntityId, new MoveIntentComponent { Dx = 0, Dy = 0 });
        componentManager.AddComponent(playerEntityId, new WorldLocationComponent { DungeonName = GameConfig.Instance.dungeonName, FloorNumber = 0 });
        componentManager.AddComponent(playerEntityId, new PlayerComponent { });

        EventDispatcher.Emit(new DungeonFloorCreationEvent(0));

        inputHandlingSystem = new InputHandlingSystem(playerEntityId, fovSystem);
    }

    public void Update()
    {
        // Phase 1: Cleanup
        EventDispatcher.ProcessEventsOfType<EntityDestructionEvent>();
        // Ensure all cleanup is done before proceeding
        if (!EventDispatcher.HasPendingEvents<EntityDestructionEvent>())
        {
            // Phase 2: Setup New Floor
            EventDispatcher.ProcessEventsOfType<LevelTransitionEvent>();
            EventDispatcher.ProcessEventsOfType<DungeonFloorCreationEvent>();
            EventDispatcher.ProcessEventsOfType<EntityInitializationEvent>();
            EventDispatcher.ProcessEventsOfType<EntityCreationEvent>();
            EventDispatcher.ProcessEventsOfType<LevelLoadedEvent>();
        }

        // Assuming setup is complete, proceed to gameplay and rendering
        if (!EventDispatcher.HasPendingEvents<DungeonFloorCreationEvent>() &&
            !EventDispatcher.HasPendingEvents<EntityInitializationEvent>() &&
            !EventDispatcher.HasPendingEvents<EntityCreationEvent>() &&
            !EventDispatcher.HasPendingEvents<LevelTransitionEvent>() &&
            !EventDispatcher.HasPendingEvents<LevelLoadedEvent>() &&
            !EventDispatcher.HasPendingEvents<EntityDestructionEvent>())
        {
            // Rendering phase
            EventDispatcher.ProcessEventsOfType<VisibilityChangeEvent>();
            EventDispatcher.ProcessEventsOfType<EntityRenderEvent>();

            // Gameplay phase
            EventDispatcher.ProcessEventsOfType<MovementIntentEvent>();
            EventDispatcher.ProcessEventsOfType<MovementCompletedEvent>();

            //Messages
            EventDispatcher.ProcessEventsOfType<MessageEvent>();
        }
    }



    public void Run()
    {
        while (isRunning)
        {
            Update();
            inputHandlingSystem.ProcessInput();
            messageLogSystem.DisplayMessages();

        }
    }
}
