public class MoveIntentComponentTemplate : ComponentTemplate
{
    private int dx, dy;

    public MoveIntentComponentTemplate(int dx, int dy)
    {
        this.dx = dx;
        this.dy = dy;
    }

    public override void AddComponentTo(int entityId, ComponentManager componentManager)
    {
        componentManager.AddComponent(entityId, new MoveIntentComponent{ Dx = dx, Dy = dy});
    }
}