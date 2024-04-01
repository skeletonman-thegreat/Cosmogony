public class ExplorationGoalComponentTemplate : ComponentTemplate
{
    private int attractiveness;
    private bool isExplored;

    public ExplorationGoalComponentTemplate(int attractiveness, bool isExplored)
    {
        this.attractiveness = attractiveness;
        this.isExplored = isExplored;
    }

    public override void AddComponentTo(int entityId, ComponentManager componentManager)
    {
        componentManager.AddComponent(entityId, new ExplorationGoalComponent{ Attractiveness = attractiveness, IsExplored = isExplored});
    }
}
