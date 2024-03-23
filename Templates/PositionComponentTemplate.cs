public class PositionComponentTemplate : ComponentTemplate
{
    private int x, y;
    private bool isValid;

    public PositionComponentTemplate(int x, int y, bool isValid = true)
    {
        this.x = x;
        this.y = y;
        this.isValid = isValid;
    }

    public override void AddComponentTo(int entityId, ComponentManager componentManager)
    {
        componentManager.AddComponent(entityId, new PositionComponent { X = x, Y = y, IsValid = isValid });
    }
}