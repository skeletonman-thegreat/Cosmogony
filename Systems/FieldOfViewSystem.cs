public class FieldOfViewSystem
{
    private ComponentManager componentManager;
    private PositionSystem positionSystem;
    private int fovRadius;
    private int width = GameConfig.Instance.gameWidth;
    private int height = GameConfig.Instance.gameHeight;
    public int fovField => fovRadius;

    public FieldOfViewSystem(ComponentManager componentManager, PositionSystem positionSystem, int fovRadius)
    {
        this.componentManager = componentManager;
        this.positionSystem = positionSystem;
        this.fovRadius = fovRadius;

        EventDispatcher.Subscribe<MovementCompletedEvent>(OnMovementCompleted);
    }

    public void OnMovementCompleted(MovementCompletedEvent e)
    {
        if(componentManager.HasComponent<PlayerComponent>(e.EntityID))
        {
            UpdateVisibility(e.EntityID);
        }
    }


    public void UpdateVisibility(int playerEntityId)
    {
        var playerPos = componentManager.GetComponent<PositionComponent>(playerEntityId);
        if (!playerPos.IsValid) return;

        for (int y = -fovRadius; y <= fovRadius; y++)
        {
            for (int x = -fovRadius; x <= fovRadius; x++)
            {
                int checkX = playerPos.X + x;
                int checkY = playerPos.Y + y;

                if (IsInBounds(checkX, checkY) && (x * x + y * y <= fovRadius * fovRadius))
                {
                    var line = GetLinePoints(playerPos.X, playerPos.Y, checkX, checkY);
                    foreach (var point in line)
                    {
                        var entityAtPos = positionSystem.GetEntityAtPosition(point.X, point.Y);
                        if (entityAtPos.HasValue)
                        {
                            if (componentManager.HasComponent<BlocksVisibilityComponent>(entityAtPos.Value))
                            {
                                // Mark the blocking entity as visible and stop processing this line
                                SetEntityVisible(entityAtPos.Value);
                                break; // Stop processing further points along this line
                            }
                            else
                            {
                                // Mark the entity as visible
                                SetEntityVisible(entityAtPos.Value);
                            }
                        }
                    }
                }
            }
        }
        EventDispatcher.Emit(new VisibilityChangeEvent(new Point(playerPos.X, playerPos.Y), fovRadius));
    }

    private void SetEntityVisible(int entityId)
    {
        var visibleComp = componentManager.GetComponent<VisibleComponent>(entityId);
        visibleComp.IsVisible = true;
        componentManager.UpdateComponent(entityId, visibleComp);
    }




    private bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }


    private List<Point> GetLinePoints(int x0, int y0, int x1, int y1)
    {
        List<Point> line = new List<Point>();

        int dx = Math.Abs(x1 - x0);
        int dy = -Math.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx + dy, e2; // error value e_xy

        while (true)
        {
            line.Add(new Point(x0, y0));
            if (x0 == x1 && y0 == y1) break;
            e2 = 2 * err;
            if (e2 >= dy) { err += dy; x0 += sx; }
            if (e2 <= dx) { err += dx; y0 += sy; }
        }

        return line;
    }
}