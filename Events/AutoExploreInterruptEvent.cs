public class AutoExploreInterruptEvent
{
    public int EntityId { get; }

    public AutoExploreInterruptEvent(int entityId)
    {
        EntityId = entityId;
    }
}