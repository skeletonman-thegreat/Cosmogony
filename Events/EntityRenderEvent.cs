public class EntityRenderEvent
{
    public Point Position { get; private set; }
    public ConsoleColor Color { get; private set; }
    public char Symbol { get; private set; }

    public EntityRenderEvent(Point position, ConsoleColor color, char symbol)
    {
        Position = position;
        Color = color;
        Symbol = symbol;
    }
}
