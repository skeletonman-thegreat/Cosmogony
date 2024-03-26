public class GameplayState : IGameState
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

    //Rendering/FOV
    private FieldOfViewSystem fovSystem;
    private Renderer renderer;

    //World System & Co
    private WorldSystem worldSystem;
    private EntityInitializationSystem entityInitializationSystem;
    private EntityFactorySystem entityFactorySystem;
    private EntityDestructionSystem entityDestructionSystem;
    private LevelTransitionSystem levelTransitionSystem;

    //Player
    private PlayerInitializationSystem playerInitializationSystem;

    public GameplayState(EntityManager entityManager, ComponentManager componentManager, MessageLogSystem messageLogSystem, PositionSystem positionSystem,
        MovementSystem movementSystem, InputHandlingSystem inputHandlingSystem, FieldOfViewSystem fovSystem, Renderer renderer, WorldSystem worldSystem,
        EntityInitializationSystem entityInitializationSystem, EntityFactorySystem entityFactorySystem, EntityDestructionSystem entityDestructionSystem, 
        LevelTransitionSystem levelTransitionSystem, PlayerInitializationSystem playerInitializationSystem)
    {
        this.entityManager = entityManager;
        this.componentManager = componentManager;
        this.messageLogSystem = messageLogSystem;
        this.positionSystem = positionSystem;
        this.movementSystem = movementSystem;
        this.inputHandlingSystem = inputHandlingSystem;
        this.fovSystem = fovSystem;
        this.renderer = renderer;
        this.worldSystem = worldSystem;
        this.entityInitializationSystem = entityInitializationSystem;
        this.entityFactorySystem = entityFactorySystem;
        this.entityDestructionSystem = entityDestructionSystem;
        this.levelTransitionSystem = levelTransitionSystem;
        this.playerInitializationSystem = playerInitializationSystem;
    }

    public void Enter()
    {
        //ECS strongmen
        entityManager = new EntityManager();
        componentManager = new ComponentManager(entityManager);

        //Message Log
        messageLogSystem = new MessageLogSystem(GameConfig.Instance.maxMessages)

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
        playerInitializationSystem = new PlayerInitializationSystem(componentManager, playerEntityId);
        componentManager.AddComponent(playerEntityId, new PriorityDrawComponent { Initialized = true });
        componentManager.AddComponent(playerEntityId, new PositionComponent { X = 0, Y = 0, IsValid = true });
        componentManager.AddComponent(playerEntityId, new RenderComponent { Symbol = '@', Color = ConsoleColor.Green });
        componentManager.AddComponent(playerEntityId, new CollisionComponent { HasCollision = true });
        componentManager.AddComponent(playerEntityId, new VisibleComponent { IsVisible = true });
        componentManager.AddComponent(playerEntityId, new MoveIntentComponent { Dx = 0, Dy = 0 });
        componentManager.AddComponent(playerEntityId, new WorldLocationComponent { DungeonName = GameConfig.Instance.dungeonName, FloorNumber = 0 });
        componentManager.AddComponent(playerEntityId, new PlayerComponent { });

        EventDispatcher.Emit(new DungeonFloorCreationEvent(0));

        inputHandlingSystem = new InputHandlingSystem(playerEntityId, fovSystem);
    }

    public void Exit()
    {
        // Cleanup, unsubscribe from events, etc.
    }

    public void Update()
    {
        // Update game logic
    }

    public void Render()
    {
        // Render game
    }
}
