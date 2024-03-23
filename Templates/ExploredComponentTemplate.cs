public class ExploredComponentTemplate : ComponentTemplate
{
    private bool isExplored;
    
    public ExploredComponentTemplate(bool  isExplored)
    {
        this.isExplored = isExplored;
    }

    public override void AddComponentTo(int entityId, ComponentManager componentManager)
    {
        componentManager.AddComponent(entityId, new ExploredComponent { IsExplored = isExplored});
    }
}