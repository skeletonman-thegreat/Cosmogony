public class InputHandlingSystem
{
    private int playerEntityID;
    private readonly FieldOfViewSystem fovSystem;

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

            switch (keyInfo.Key)
            {
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
                    EventDispatcher.Emit(new LevelTransitionEvent(TransitionReason.Stairs, TransitionDirection.Descend));
                    break;
                case ConsoleKey.OemComma:
                    EventDispatcher.Emit(new LevelTransitionEvent(TransitionReason.Stairs, TransitionDirection.Ascend));
                    break;
                default:
                    break;
            }
        }
    }

private void EmitMoveIntent(int dx, int dy)
    {
        EventDispatcher.Emit(new MovementIntentEvent(playerEntityID, dx, dy));
    }
}