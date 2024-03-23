public class CollisionComponentTemplate : ComponentTemplate
{
    private bool hasCollision; 

    public CollisionComponentTemplate(bool hasCollision)
    {
        this.hasCollision = hasCollision;
    }

    public override void AddComponentTo(int entityId, ComponentManager componentManager)
    {
        componentManager.AddComponent(entityId, new CollisionComponent { HasCollision = hasCollision });
    }
}
