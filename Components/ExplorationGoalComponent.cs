public struct ExplorationGoalComponent
{
    public int Attractiveness;
    public bool IsExplored { get; set; }
    public bool Initialized { get; set; }

    public ExplorationGoalComponent(int attractiveness, bool isExplored)
    {
        Attractiveness = attractiveness;
        IsExplored = isExplored;
    }
}
