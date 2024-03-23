class Program
{

    static void Main(string[] args)
    {
        Console.CursorVisible = false;

        int consoleWidth = GameConfig.Instance.consoleWidth;
        int consoleHeight = GameConfig.Instance.consoleHeight;

        try
        {
            Console.BufferWidth = consoleWidth;
            Console.BufferHeight = consoleHeight;

            Console.WindowWidth = consoleWidth;
            Console.WindowHeight = consoleHeight;
        }
        catch(ArgumentOutOfRangeException e)
        {
            Console.WriteLine("Error Setting Console Window Size: " +  e.Message);
        }
        Game game = new Game();
        game.Run();
    }
}
