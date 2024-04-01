public static class EventDispatcher
{
    private static Dictionary<Type, List<Delegate>> listeners = new Dictionary<Type, List<Delegate>>();
    private static readonly Dictionary<Type, Queue<object>> eventQueues = new Dictionary<Type, Queue<object>>();

    public static void Subscribe<T>(Action<T> listener) where T : class
    {
        var type = typeof(T);
        if (!listeners.ContainsKey(type))
        {
            listeners[type] = new List<Delegate>();
        }
        listeners[type].Add(listener);
    }


    public static void Emit<T>(T eventInstance) where T : class
    {
        var type = typeof(T);
        if (listeners.ContainsKey(type))
        {
            foreach (var delegateObj in listeners[type])
            {
                // Safely cast back to the correct delegate type.
                var listener = (Action<T>)delegateObj;
                listener(eventInstance);
            }
        }
    }

    // Process all events for a specific type
    public static void ProcessEventsOfType<T>() where T : class
    {
        var type = typeof(T);
        if (eventQueues.TryGetValue(type, out var queue))
        {
            while (queue.Count > 0)
            {
                var eventInstance = (T)queue.Dequeue(); // Assume the queue is Queue<object> for generic handling
                if (listeners.TryGetValue(type, out var eventListeners))
                {
                    foreach (Delegate del in eventListeners)
                    {
                        var listener = (Action<T>)del;
                        listener(eventInstance);
                    }
                }
            }
        }
    }

    // Check if there are any pending events for a specific type
    public static bool HasPendingEvents<T>() where T : class
    {
        var type = typeof(T);
        if (eventQueues.TryGetValue(type, out var queue))
        {
            return queue.Count > 0;
        }
        return false;
    }
}
