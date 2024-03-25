public class ComponentArray<T> : IComponentArray where T : struct
{
    private T[] components;
    private int[] entityToIndex; //Maps entity IDs to component array indices
    private int[] indexToEntity; //Maps compenent array indices back to entity IDs
    private int size;

    public ComponentArray(int maxEntities)
    {
        components = new T[maxEntities];
        entityToIndex = new int[maxEntities];
        indexToEntity = new int[maxEntities];
        size = 0;

        //Initialize mappings to indicate "no component"
        Array.Fill(entityToIndex, -1);
        Array.Fill(indexToEntity, -1);
    }

    public void AddComponent(int entityID, T component)
    {
        components[size] = component;
        entityToIndex[entityID] = size;
        indexToEntity[size] = entityID;
        size++;
    }

    public void RemoveComponent(int entityID)
    {
        int indexToRemove = entityToIndex[entityID];
        if (indexToRemove == -1) return; //Entity does not have component

        //Move last compenent into the vacated spot to keep array dense
        components[indexToRemove] = components[size - 1];
        int lastEntityID = indexToEntity[size - 1];
        entityToIndex[lastEntityID] = indexToRemove;
        indexToEntity[indexToRemove] = lastEntityID;

        //Update Mappings
        entityToIndex[entityID] = -1;
        indexToEntity[size - 1] = -1;
        size--;
    }

    public void UpdateComponent(int entityID, T component)
    {
        int index = entityToIndex[entityID];
        if(index != -1)
        {
            components[index] = component;
        }
        else
        {
            throw new Exception("Entity does not have the component to update.");
        }
    }

    public T GetComponent(int entityID)
    {
        int index = entityToIndex[entityID];
        return index != -1 ? components[index] : throw new Exception("Entity does not have component.");
    }

    public IEnumerable<T> GetAllCompenents()
    {
        for(int i = 0; i < size; i++)
        {
            yield return components[i];
        }
    }

    public bool HasComponent(int entityID)
    {
        int index = entityToIndex[entityID];
        return (index != -1);
    }

    public IEnumerable<int> GetAllEntities()
    {
        for (int i = 0; i < size; i++)
        {
            yield return indexToEntity[i];
        }
    }

    void IComponentArray.AddComponent(int entityId)
    {
        throw new NotImplementedException();
    }

    void IComponentArray.RemoveComponent(int entityId)
    {
        this.RemoveComponent(entityId);
    }

    public void UpdateComponent(int entityId, object component)
    {
        if (component is T typedComponent)
        {
            UpdateComponent(entityId, typedComponent); // Call the generic version
        }
        else
        {
            throw new ArgumentException($"Component type mismatch. Expected type {typeof(T).Name}.");
        }
    }

    bool IComponentArray.HasComponent(int entityId)
    {
        return this.HasComponent(entityId);
    }
}