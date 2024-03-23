public class ComponentManager
{
    private EntityManager entityManager;
    private Dictionary<Type, object> componentArrays = new Dictionary<Type, object>();

    public ComponentManager(EntityManager entityManager)
    {
        this.entityManager = entityManager;
    }

    public void AddComponent<T>(int entityId, T component) where T : struct
    {
        var type = typeof(T);

        // Check if a ComponentArray for this type already exists
        if (!componentArrays.ContainsKey(type))
        {
            // If it doesn't, create and store a new ComponentArray for this type
            componentArrays[type] = new ComponentArray<T>(entityManager.MaxEntities);
        }

        // Retrieve the ComponentArray from the dictionary and add the component to it
        var componentArray = (ComponentArray<T>)componentArrays[type];
        componentArray.AddComponent(entityId, component);
    }


    public void RemoveComponent<T>(int entityId) where T : struct
    {
        GetComponentArray<T>().RemoveComponent(entityId);
    }

    public T GetComponent<T>(int entityId) where T : struct
    {
        return GetComponentArray<T>().GetComponent(entityId);
    }

    private ComponentArray<T> GetComponentArray<T>() where T : struct
    {
        Type type = typeof(T);
        if (!componentArrays.ContainsKey(type))
        {
            componentArrays[type] = new ComponentArray<T>(entityManager.MaxEntities);
        }
        return (ComponentArray<T>)componentArrays[type];
    }

    public IEnumerable<int> GetAllEntitiesWithComponent<T>() where T : struct
    {
        if (componentArrays.TryGetValue(typeof(T), out var componentArray))
        {
            var typedArray = (ComponentArray<T>)componentArray;
            foreach (var entity in typedArray.GetAllEntities())
            {
                yield return entity;
            }
        }
    }

    public bool HasComponent<T>(int entityId) where T : struct
    {
        if (!componentArrays.TryGetValue(typeof(T), out var componentArrayObj))
        {
            return false; // Component type not registered, so entity cannot have it
        }

        var componentArray = (ComponentArray<T>)componentArrayObj;
        return componentArray.HasComponent(entityId);
    }


    public bool HasComponents<T1, T2>(int entityId) where T1 : struct where T2 : struct
    {
        return HasComponent<T1>(entityId) && HasComponent<T2>(entityId);
    }

    public IEnumerable<int> GetAllEntitiesWithComponents<T1, T2>() where T1 : struct where T2 : struct
    {
        // Use the refined HasComponents method
        for (int entityId = 0; entityId < entityManager.MaxEntities; entityId++)
        {
            if (HasComponents<T1, T2>(entityId))
            {
                yield return entityId;
            }
        }
    }


    public void UpdateComponent<T>(int entityId, T component) where T : struct
    {
        var type = typeof(T);

        if (!componentArrays.ContainsKey(type))
        {
            throw new Exception($"Component type {type} not found.");
        }

        var componentArray = (ComponentArray<T>)componentArrays[type];
        componentArray.UpdateComponent(entityId, component);
    }



    // Optionally, implement methods to query entities by components, etc.
}
