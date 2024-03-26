public class GameStateManager
{
    private Dictionary<GameState, IGameState> states;
    private IGameState currentState;

    public GameStateManager()
    {
        states = new Dictionary<GameState, IGameState>();
        // Initialize and add game states here
        // For example: states.Add(GameState.TitleScreen, new TitleScreenState());
    }

    public void ChangeState(GameState newState)
    {
        currentState?.Exit();
        currentState = states[newState];
        currentState.Enter();
    }

    public void Update()
    {
        currentState?.Update();
    }

    public void Render()
    {
        currentState?.Render();
    }
}
