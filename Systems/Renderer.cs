public class Renderer
{
    ComponentManager componentManager;
    public Renderer(ComponentManager componentManager)
    {
        // Wrap the method call in a lambda that casts the event to the correct type
        //EventDispatcher.Subscribe<EntityRenderEvent>(e => OnEntityRenderEvent((EntityRenderEvent)e));
        EventDispatcher.Subscribe<VisibilityChangeEvent>(OnVisibilityChangeEvent);
        this.componentManager = componentManager;
    }

    private void OnVisibilityChangeEvent(VisibilityChangeEvent e)
    {
        Console.Clear();

        RenderTerrain();
        RenderOverlappableEntities();

        Console.ResetColor();
    }

    private void RenderTerrain()
    {
        var terrainEntities = componentManager.GetAllEntitiesWithComponent<VisibleComponent>()
            .Where(entity => componentManager.HasComponent<TerrainComponent>(entity) && componentManager.GetComponent<VisibleComponent>(entity).IsVisible);

        foreach (var entity in terrainEntities)
        {
            RenderEntity(entity);
        }
    }

    private void RenderOverlappableEntities()
    {
        var overlappableEntities = componentManager.GetAllEntitiesWithComponent<VisibleComponent>()
            .Where(entity => componentManager.HasComponent<PriorityDrawComponent>(entity) && componentManager.GetComponent<VisibleComponent>(entity).IsVisible);

        foreach (var entity in overlappableEntities)
        {
            RenderEntity(entity);
        }
    }

    private void RenderEntity(int entity)
    {
        var position = componentManager.GetComponent<PositionComponent>(entity);
        var render = componentManager.GetComponent<RenderComponent>(entity);

        Console.SetCursorPosition(position.X, position.Y);
        Console.ForegroundColor = render.Color;
        Console.Write(render.Symbol);
    }

}
