public class RenderComponentTemplate : ComponentTemplate
{
    private char symbol;
    private ConsoleColor color;

    public RenderComponentTemplate(char symbol, ConsoleColor color)
    {
        this.symbol = symbol;
        this.color = color;
    }

    public override void AddComponentTo(int entityId, ComponentManager componentManager)
    {
        componentManager.AddComponent(entityId, new RenderComponent{ Symbol = symbol, Color = color });
    }
}
