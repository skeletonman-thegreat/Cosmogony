public class LevelTransitionEvent
{
    public int TargetLevel { get; }
    public TransitionReason Reason { get; }
    public TransitionDirection Direction { get; }

    public LevelTransitionEvent(TransitionReason reason, TransitionDirection direction)
    {
        Reason = reason;
        Direction = direction;
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
