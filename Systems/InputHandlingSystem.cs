public class InputHandlingSystem
{
    private int playerEntityID;
    private readonly FieldOfViewSystem fovSystem;
    private bool isAutoExploring = false;

    public InputHandlingSystem(int playerEntityID, FieldOfViewSystem fovSystem)
    {
        this.playerEntityID = playerEntityID;
        this.fovSystem = fovSystem;
    }

    public void ProcessInput()
    {
        if (Console.KeyAvailable)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

            if (isAutoExploring && keyInfo.Key != ConsoleKey.Z)
            {
                isAutoExploring = false; // Stop auto-exploration
                EventDispatcher.Emit(new AutoExploreInterruptEvent(playerEntityID));
                return; // Optionally return early to ignore other inputs during cancellation
            }

            switch (keyInfo.Key)
            {
                case ConsoleKey.Z:
                    // Toggle auto-exploration state
                    isAutoExploring = !isAutoExploring;
                    if (isAutoExploring)
                    {
                        // Start auto-exploration
                        EventDispatcher.Emit(new AutoExploreEvent(playerEntityID));
                    }
                    else
                    {
                        // Manually cancel auto-exploration
                        EventDispatcher.Emit(new AutoExploreInterruptEvent(playerEntityID));
                    }
                    break;
                case ConsoleKey.NumPad1:
                    EmitMoveIntent(-1, 1);
                    break;
                case ConsoleKey.NumPad2:
                    EmitMoveIntent(0, 1);
                    break;
                case ConsoleKey.NumPad3:
                    EmitMoveIntent(1, 1);
                    break;
                case ConsoleKey.NumPad4:
                    EmitMoveIntent(-1, 0);
                    break;
                case ConsoleKey.NumPad5:
                    // Stay in place (no movement)
                    EmitMoveIntent(0, 0);
                    break;
                case ConsoleKey.NumPad6:
                    EmitMoveIntent(1, 0);
                    break;
                case ConsoleKey.NumPad7:
                    EmitMoveIntent(-1, -1);
                    break;
                case ConsoleKey.NumPad8:
                    EmitMoveIntent(0, -1);
                    break;
                case ConsoleKey.NumPad9:
                    EmitMoveIntent(1, -1);
                    break;
                case ConsoleKey.OemPeriod:
                    EventDispatcher.Emit(new LevelTransitionEvent(TransitionReason.Stairs, TransitionDirection.Descend, playerEntityID));
                    break;
                case ConsoleKey.OemComma:
                    EventDispatcher.Emit(new LevelTransitionEvent(TransitionReason.Stairs, TransitionDirection.Ascend, playerEntityID));
                    break;
                default:
                    // If any key other than the auto-explore toggle is pressed, ensure auto-explore is stopped
                    if (isAutoExploring)
                    {
                        isAutoExploring = false;
                        EventDispatcher.Emit(new AutoExploreInterruptEvent(playerEntityID));
                    }
                    break;
            }
        }
    }

private void EmitMoveIntent(int dx, int dy)
    {
        EventDispatcher.Emit(new MovementIntentEvent(playerEntityID, dx, dy));
    }
}