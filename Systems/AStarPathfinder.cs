public class AStarPathfinder
{
    private char[,] map;

    public AStarPathfinder(char[,] map)
    {
        this.map = map;
    }

    public List<Point> FindPath(Point start, Point goal)
    {
        var openList = new List<Node>();
        var closedList = new HashSet<Point>();
        openList.Add(new Node(start, 0, GetHeuristic(start, goal), null));

        while (openList.Count > 0)
        {
            var current = openList.OrderBy(node => node.TotalCost).First();
            if (current.Position.X == goal.X && current.Position.Y == goal.Y)
            {
                return ConstructPath(current);
            }

            openList.Remove(current);
            closedList.Add(current.Position);

            foreach (var neighbor in GetNeighbors(current.Position))
            {
                if (closedList.Contains(neighbor) || IsObstacle(neighbor)) continue;

                var costToNeighbor = current.Cost + 1; // Assuming uniform cost for simplicity
                var neighborNode = openList.FirstOrDefault(n => n.Position.X == neighbor.X && n.Position.Y == neighbor.Y);

                if (neighborNode == null)
                {
                    neighborNode = new Node(neighbor, costToNeighbor, GetHeuristic(neighbor, goal), current);
                    openList.Add(neighborNode);
                }
                else if (costToNeighbor < neighborNode.Cost)
                {
                    neighborNode.Cost = costToNeighbor;
                    neighborNode.Parent = current;
                }
            }
        }

        return new List<Point>(); // No path found
    }

    private List<Point> ConstructPath(Node node)
    {
        var path = new List<Point>();
        while (node != null)
        {
            path.Add(node.Position);
            node = node.Parent;
        }
        path.Reverse();
        return path;
    }

    private bool IsObstacle(Point point)
    {
        return map[point.X, point.Y] == '#';
    }

    private float GetHeuristic(Point point, Point goal)
    {
        // Manhattan distance
        return Math.Abs(point.X - goal.X) + Math.Abs(point.Y - goal.Y);
    }

    private IEnumerable<Point> GetNeighbors(Point point)
    {
        var directions = new[]
        {
            //cardinals
            new Point(0, -1), // N
            new Point(1, 0),  // E
            new Point(0, 1),  // S
            new Point(-1, 0),  // W
            //ordinals
            new Point(-1, -1), //NW
            new Point(1, -1), //NE
            new Point(-1, 1), //SW
            new Point(1, 1) //SE
        };

        foreach (var dir in directions)
        {
            var next = new Point(point.X + dir.X, point.Y + dir.Y);
            if (next.X >= 0 && next.X < map.GetLength(0) && next.Y >= 0 && next.Y < map.GetLength(1))
            {
                yield return next;
            }
        }
    }
}
