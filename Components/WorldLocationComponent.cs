public struct WorldLocationComponent
{
    public string DungeonName;
    public int FloorNumber;
    public bool Initialized { get; set; }

    public WorldLocationComponent(string dungeonName, int floorNumber)
    {
        DungeonName = dungeonName;
        FloorNumber = floorNumber;
    }
}