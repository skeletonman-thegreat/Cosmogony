public class EventDispatcher
{
    private static Dictionary<Type, List<Action<object>>> listeners = new Dictionary<Type, List<Action<object>>>();

    public static void Subscribe<T>(Action<object> listener)
    {
        var type = typeof(T);
        if (!listeners.ContainsKey(type))
        {
            listeners[type] = new List<Action<object>>();
        }
        listeners[type].Add(listener);
    }

    public static void Emit<T>(T eventInstance)
    {
        var type = typeof(T);
        if (listeners.ContainsKey(type))
        {
            foreach (var listener in listeners[type])
            {
                listener(eventInstance);
            }
        }
    }
}
