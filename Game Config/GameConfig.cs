public class GameConfig
{
    private static readonly GameConfig instance = new GameConfig();
    public static GameConfig Instance => instance;

    public int consoleWidth {  get; private set; }

    public int consoleHeight { get; private set; }

    public int gameWidth { get; private set; }
    public int gameHeight { get; private set; }

    public int messageWidth { get; private set; }

    public int maxMessages {  get; private set; }

    public string dungeonName { get; private set; }

    public bool playerInputEnabled { get; set; }

    // Private constructor to prevent external instantiation
    private GameConfig()
    {
        // Initialize with default values
        consoleWidth = 110;
        consoleHeight = 40;
        gameWidth = 80; // Default width
        gameHeight = 25; // Default height
        messageWidth = consoleWidth - gameWidth;
        maxMessages = 40;
        dungeonName = "Proving Grounds";
        playerInputEnabled = true;
    }

    // Method to configure the dimensions if needed
    public void Configure(int consoleWidth, int consoleHeight, int gameWidth, int gameHeight)
    {
        consoleWidth = this.consoleWidth;
        consoleHeight = this.consoleHeight;
        gameWidth = this.gameWidth;
        gameHeight = this.gameHeight;
    }
}
