public class LevelTransitionEvent
{
    public int TargetLevel { get; }
    public int EntityId;
    public TransitionReason Reason { get; }
    public TransitionDirection Direction { get; }

    public LevelTransitionEvent(TransitionReason reason, TransitionDirection direction, int entityId)
    {
        Reason = reason;
        Direction = direction;
        EntityId = entityId;
    }
}

public enum TransitionReason
{
    Stairs,
    Teleport,
    Forced // e.g., being pulled by an enemy or a trap
}

public enum TransitionDirection
{
    Ascend,
    Descend
}
