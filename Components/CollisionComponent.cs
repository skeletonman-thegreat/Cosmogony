public struct CollisionComponent
{
    public bool HasCollision;
    public bool Initialized { get; set; }

    public CollisionComponent(bool hascollision)
    {
        HasCollision = hascollision;
    }
}