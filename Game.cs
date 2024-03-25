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

    public bool isRunning { get; private set; }

    public Game()
    {
        //ECS strongmen
        entityManager = new EntityManager();
        componentManager = new ComponentManager(entityManager);

        //Message Log
        messageLogSystem = new MessageLogSystem(GameConfig.Instance.maxMessages);
        EventDispatcher.Subscribe<MessageEvent>(OnMessageReceived);

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


        //Start the main game loop
        InitializeGame();
        isRunning = true;
    }

    //hey ding dong, if you're reading this, you need to actually create some methods or something to actually USE your systems
    //ffs dude, get your shit together!

    public void InitializeGame()
    {
        LogGameMessage("Game is loading, please wait patiently! (:");

        //Player
        int playerEntityId = entityManager.CreateEntity();
        playerInitializationSystem = new PlayerInitializationSystem(componentManager, playerEntityId);
        componentManager.AddComponent(playerEntityId, new PriorityDrawComponent {Initialized = true });
        componentManager.AddComponent(playerEntityId, new PositionComponent { X = 0, Y = 0, IsValid = true });
        componentManager.AddComponent(playerEntityId, new RenderComponent { Symbol = '@', Color = ConsoleColor.Green });
        componentManager.AddComponent(playerEntityId, new CollisionComponent { HasCollision = true });
        componentManager.AddComponent(playerEntityId, new VisibleComponent { IsVisible = true });
        componentManager.AddComponent(playerEntityId, new MoveIntentComponent { Dx = 0, Dy = 0 });
        componentManager.AddComponent(playerEntityId, new WorldLocationComponent { DungeonName = GameConfig.Instance.dungeonName, FloorNumber = 0 });
        componentManager.AddComponent(playerEntityId, new PlayerComponent { });

        EventDispatcher.Emit(new DungeonFloorCreationEvent(0));

        inputHandlingSystem = new InputHandlingSystem(playerEntityId, fovSystem);

        //fovSystem.UpdateVisibility(playerEntityId);

        // Further initialization as needed...
    }

    public void OnMessageReceived(object e)
    {
        var eventInstance = (MessageEvent)e;
        //Log the message to the message system
        messageLogSystem.LogMessage(eventInstance.Message);
    }

    public void LogGameMessage(string message)
    {
        // Emit a message event
        EventDispatcher.Emit(new MessageEvent(message));
    }


    public void Run()
    {
        while (isRunning)
        {
            if(!isRunning) break;
            messageLogSystem.DisplayMessages();
            if(GameConfig.Instance.playerInputEnabled) { inputHandlingSystem.ProcessInput(); }

            Thread.Sleep(20); //time to sleep to prevent loop from running too fast!
        }
    }
}
