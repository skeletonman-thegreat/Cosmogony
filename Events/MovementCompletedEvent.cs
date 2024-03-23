public class MovementCompletedEvent
{
    public int EntityID { get; }
    public int NewX { get; }
    public int NewY { get; }

    public MovementCompletedEvent(int entityID, int newX, int newY)
    {
        EntityID = entityID;
        NewX = newX;
        NewY = newY;
    }


}