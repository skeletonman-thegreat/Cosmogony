public class Edge
{
    public int Room1Index { get; set; }
    public int Room2Index { get; set; }
    public int Weight { get; set; }

    public Edge(int room1Index, int room2Index, int weight)
    {
        Room1Index = room1Index;
        Room2Index = room2Index;
        Weight = weight;
    }
}