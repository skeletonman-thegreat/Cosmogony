public struct RenderComponent
{
    public char Symbol;
    public ConsoleColor Color;
    public bool Initialized { get; set; }

    public RenderComponent(char symbol, ConsoleColor color)
    {
        Symbol = symbol;
        Color = color;
    }
}