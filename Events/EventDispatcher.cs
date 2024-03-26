public static class EventDispatcher
{
    private static Dictionary<Type, List<Action<object>>> listeners = new Dictionary<Type, List<Action<object>>>();
    private static Dictionary<Type, Queue<object>> eventQueues = new Dictionary<Type, Queue<object>>();

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
        if (!eventQueues.ContainsKey(type))
        {
            eventQueues[type] = new Queue<object>();
        }
        eventQueues[type].Enqueue(eventInstance);
    }

    // Process all events for a specific type
    public static void ProcessEventsOfType<T>() where T : class
    {
        var type = typeof(T);
        if (eventQueues.TryGetValue(type, out var queue))
        {
            while (queue.Count > 0)
            {
                var eventInstance = queue.Dequeue();
                if (listeners.ContainsKey(type))
                {
                    foreach (var listener in listeners[type])
                    {
                        listener(eventInstance);
                    }
                }
            }
        }
    }

    // Optionally: Method to process a single event of each type, if you want to spread out processing
    public static void ProcessOneEventPerType()
    {
        foreach (var type in eventQueues.Keys.ToList())
        {
            if (eventQueues[type].Count > 0)
            {
                var eventInstance = eventQueues[type].Dequeue();
                if (listeners.ContainsKey(type))
                {
                    foreach (var listener in listeners[type])
                    {
                        listener(eventInstance);
                    }
                }
            }
        }
    }
}
