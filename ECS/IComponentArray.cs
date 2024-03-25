public interface IComponentArray
{
    void AddComponent(int entityId);
    void RemoveComponent(int entityId);
    void UpdateComponent(int entityId, object component);
    bool HasComponent(int entityId);

}