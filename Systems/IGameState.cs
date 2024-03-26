public interface IGameState
{
    void Enter(); // Called when entering the state
    void Exit(); // Called when exiting the state
    void Update(); // Update the state logic
    void Render(); // Handle state rendering
}
