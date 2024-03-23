public struct PositionComponent
{
    public int X;
    public int Y;
    public bool IsValid; // Add a flag to indicate if this component is valid
    public bool Initialized { get; set; }

    public PositionComponent(int x, int y, bool isValid = true)
    {
        X = x;
        Y = y;
        IsValid = isValid;
    }
}