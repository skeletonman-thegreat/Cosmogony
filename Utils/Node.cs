public class Node
{
    public Point Position { get; set; }
    public float Cost { get; set; }
    public float Heuristic { get; set; }
    public float TotalCost => Cost + Heuristic;
    public Node Parent { get; set; }

    public Node(Point position, float cost, float heuristic, Node parent)
    {
        Position = position;
        Cost = cost;
        Heuristic = heuristic;
        Parent = parent;
    }
}

public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}
