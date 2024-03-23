public class MovementIntentEvent
{
    public int EntityID { get; }
    public int Dx { get; }
    public int Dy { get; }

    public MovementIntentEvent(int entityID, int dx, int dy)
    {
        EntityID = entityID;
        Dx = dx;
        Dy = dy;
    }
}