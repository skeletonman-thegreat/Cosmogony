public class EntityDestructionEvent
{
    public int EntityID { get; }
    public EntityDestructionEvent(int entityID)
    {
        EntityID = entityID;
    }
}
