public class VisibleComponentTemplate : ComponentTemplate
{
    private bool isVisible;

    public VisibleComponentTemplate(bool isVisible)
    {
        this.isVisible = isVisible;
    }

    public override void AddComponentTo(int entityId, ComponentManager componentManager)
    {
        componentManager.AddComponent(entityId, new VisibleComponent { IsVisible = isVisible });
    }
}