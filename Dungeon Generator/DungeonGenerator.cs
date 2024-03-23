using System.Drawing;
using System.Numerics;

public class DungeonGenerator
{
    //Constants
    private const int MIN_ROOM_SIZE = 2;
    private const int ROOM_PADDING = 1;
    private const int MAX_DEPTH = 5;

    private int width = GameConfig.Instance.gameWidth;
    private int height = GameConfig.Instance.gameHeight;
    public int Width => width;
    public int Height => height;
    private char[,] dungeonMap;
    private List<Rectangle> rooms = [];
    private Random random;

    public DungeonGenerator()
    {
        dungeonMap = new char[width, height];
        random = new Random();
    }


    public char[,] GenerateDungeon()
    {
        InitializeDungeonMap();
        BSP(0, 0, width - 1, height - 1, 0);
        ConnectRoomsUsingMst();
        PlaceAscentPoint();
        PlaceDescentPoint();
        return dungeonMap;
    }

    private void InitializeDungeonMap()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                dungeonMap[x, y] = '#';
            }
        }
    }

    private void BSP(int x1, int y1, int x2, int y2, int depth)
    {
        if (depth >= MAX_DEPTH || AreaTooSmall(x1, y1, x2, y2))
        {
            CreateRoom(x1, y1, x2, y2);
            return;
        }

        bool splitVertically = ShouldSplitVertically(x1, y1, x2, y2);
        int split = splitVertically ? random.Next(x1 + MIN_ROOM_SIZE, x2 - MIN_ROOM_SIZE)
                                     : random.Next(y1 + MIN_ROOM_SIZE, y2 - MIN_ROOM_SIZE);

        if (splitVertically)
        {
            BSP(x1, y1, split, y2, depth + 1);
            BSP(split + 1, y1, x2, y2, depth + 1);
        }
        else
        {
            BSP(x1, y1, x2, split, depth + 1);
            BSP(x1, split + 1, x2, y2, depth + 1);
        }
    }

    private bool AreaTooSmall(int x1, int y1, int x2, int y2)
    {
        return (x2 - x1 < MIN_ROOM_SIZE * 2 + ROOM_PADDING * 2) || (y2 - y1 < MIN_ROOM_SIZE * 2 + ROOM_PADDING * 2);
    }

    private bool ShouldSplitVertically(int x1, int y1, int x2, int y2)
    {
        if ((x2 - x1) > (y2 - y1))
        {
            return random.Next(100) < 75; // Favor vertical split if room is wider
        }
        else
        {
            return random.Next(100) >= 75; // Favor horizontal split if room is taller or equal
        }
    }

    private void CreateRoom(int x1, int y1, int x2, int y2)
    {
        // Room creation logic with padding adjustment
        // Ensure the room is within bounds and respects the MIN_ROOM_SIZE after padding adjustment
        x1 += ROOM_PADDING;
        y1 += ROOM_PADDING;
        x2 -= ROOM_PADDING;
        y2 -= ROOM_PADDING;

        // Ensure room dimensions respect MIN_ROOM_SIZE
        if (x2 - x1 < MIN_ROOM_SIZE || y2 - y1 < MIN_ROOM_SIZE)
        {
            return; // Room too small to be created after padding adjustment
        }

        // Add the room to the list of rooms
        rooms.Add(new Rectangle(x1, y1, x2 - x1, y2 - y1));

        // Fill in the room on the map
        for (int y = y1; y <= y2; y++)
        {
            for (int x = x1; x <= x2; x++)
            {
                dungeonMap[x, y] = '.';
            }
        }
    }

    private void ConnectRoomsUsingMst()
    {
        List<Edge> edges = new List<Edge>();
        //Generate all possible "edges" (corridors) between rooms
        for(int i = 0; i < rooms.Count; i++)
        {
            for(int j = i + 1;  j < rooms.Count; j++)
            {
                int distance = CalculateDistance(rooms[i], rooms[j]);
                edges.Add(new Edge(i, j, distance));

            }
        }
        //sort edges by "weight" (distance)
        edges.Sort((a, b) => a.Weight.CompareTo(b.Weight));

        UnionFind uf = new UnionFind(rooms.Count);
        foreach(Edge edge in edges)
        {
            if(uf.Find(edge.Room1Index) != uf.Find(edge.Room2Index))
            {
                uf.Union(edge.Room1Index, edge.Room2Index);
                CreateCorridor(rooms[edge.Room1Index], rooms[edge.Room2Index]);
            }
        }
    }

    private Point GetCenter(Rectangle room)
    {
        return new Point((room.Left + room.Right) / 2, (room.Top + room.Bottom) / 2);
    }

    private int CalculateDistance(Rectangle room1, Rectangle room2)
    {
        Point center1 = GetCenter(room1);
        Point center2 = GetCenter(room2);
        return Math.Abs(center1.X - center2.X) + Math.Abs(center1.Y - center2.Y); //Manhattan Distance
    }

    private void CreateCorridor(Rectangle room1, Rectangle room2)
    {
        Point center1 = GetCenter(room1);
        Point center2 = GetCenter(room2);

        // Randomly choose to start horizontally or vertically
        bool horizontalFirst = random.Next(2) == 0;

        if (horizontalFirst)
        {
            // Create horizontal corridor
            CreateHorizontalCorridor(center1.X, center2.X, center1.Y);

            // Create vertical corridor
            CreateVerticalCorridor(center1.Y, center2.Y, center2.X);
        }
        else
        {
            // Create vertical corridor
            CreateVerticalCorridor(center1.Y, center2.Y, center1.X);

            // Create horizontal corridor
            CreateHorizontalCorridor(center1.X, center2.X, center2.Y);
        }
    }

    private void CreateHorizontalCorridor(int xStart, int xEnd, int yPosition)
    {
        int min = Math.Min(xStart, xEnd);
        int max = Math.Max(xStart, xEnd);

        for (int x = min; x <= max; x++)
        {
            if (x >= 0 && x < width && yPosition >= 0 && yPosition < height)
            {
                dungeonMap[x, yPosition] = '.';
            }
        }
    }

    private void CreateVerticalCorridor(int yStart, int yEnd, int xPosition)
    {
        int min = Math.Min(yStart, yEnd);
        int max = Math.Max(yStart, yEnd);

        for (int y = min; y <= max; y++)
        {
            if (xPosition >= 0 && xPosition < width && y >= 0 && y < height)
            {
                dungeonMap[xPosition, y] = '.';
            }
        }
    }


    public Point PlaceAscentPoint()
    {
        if (rooms.Count > 0)
        {
            // Assuming you use the first room for the spawn point
            int RoomIndex = random.Next(rooms.Count);
            Rectangle room = rooms[RoomIndex];

            // Calculate a position for the spawn point, e.g., the center of the first room
            int spawnX = (room.Left + room.Right) / 2;
            int spawnY = (room.Top + room.Bottom) / 2;

            // Place the '<' icon at the spawn position
            dungeonMap[spawnX, spawnY] = '<';

            // Return the spawn position
            return new Point(spawnX, spawnY);
        }

        // Fallback if no rooms are available
        return new Point(-1, -1); // Indicates failure to place the spawn point
    }

    public Point PlaceDescentPoint()
    {
        int RoomIndex = random.Next(rooms.Count);
        Rectangle room = rooms[RoomIndex];

        int x = random.Next(room.Left + 1, room.Right);
        int y = random.Next(room.Top + 1, room.Bottom);

        dungeonMap[x, y] = '>';
        return new Point(x, y);
    }

    public bool DFS(char[,] map, Point start, Point end)
    {
        int width = map.GetLength(0);
        int height = map.GetLength(1);
        bool[,] visited = new bool[width, height]; // Tracks visited cells

        return DFSVisit(map, start, end, visited);
    }

    private bool DFSVisit(char[,] map, Point current, Point end, bool[,] visited)
    {
        if (current.X < 0 || current.X >= map.GetLength(0) || current.Y < 0 || current.Y >= map.GetLength(1))
            return false; // Out of bounds

        if (visited[current.X, current.Y] || map[current.X, current.Y] == '#')
            return false; // Already visited or not walkable

        if (current.Equals(end))
            return true; // End point reached

        visited[current.X, current.Y] = true; // Mark as visited

        // Explore neighbors (down, right, up, left)
        var directions = new Point[] { new Point(0, 1), new Point(1, 0), new Point(0, -1), new Point(-1, 0) };
        foreach (var dir in directions)
        {
            Point next = new Point(current.X + dir.X, current.Y + dir.Y);
            if (DFSVisit(map, next, end, visited))
                return true; // Path found
        }

        return false; // No path found
    }

}
