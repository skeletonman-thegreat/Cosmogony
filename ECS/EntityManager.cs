public class EntityManager
{
    private const int MAX_ENTITIES = 1000000;
    private Stack<int> freeIDs = new Stack<int>();
    private int nextID = 0;
    private int activeEntityCount = 0;
    public int MaxEntities => MAX_ENTITIES;

    public EntityManager()
    {

    }

    public int CreateEntity()
    {
        int id = freeIDs.Count > 0 ? freeIDs.Pop() : nextID++;
        if (id >= MAX_ENTITIES) throw new Exception("Maximum number of entities exceeded!");
        activeEntityCount++;
        return id;
    }

    public void DestroyEntity(int entityId)
    {
        freeIDs.Push(entityId);
        activeEntityCount--;
    }


    public int ActiveEntityCount => activeEntityCount;
}
