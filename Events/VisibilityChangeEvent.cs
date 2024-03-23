public class VisibilityChangeEvent
{
    public Point PlayerPosition { get; private set; }
    public int FovRadius { get; private set; }

    public VisibilityChangeEvent(Point playerPosition, int fovRadius)
    {
        PlayerPosition = playerPosition;
        FovRadius = fovRadius;
    }
}
