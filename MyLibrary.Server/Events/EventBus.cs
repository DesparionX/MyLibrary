namespace MyLibrary.Server.Events
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> _handlers = new();

        public static void Subscribe<T>(Action<T> handler)
        {
            if (!_handlers.ContainsKey(typeof(T)))
            {
                _handlers[typeof(T)] = new List<Delegate>();
            }
            _handlers[typeof(T)].Add(handler);
        }

        public static void Publish<T>(T @event)
        {
            if(!_handlers.TryGetValue(typeof(T), out var handlers))
            {
                foreach (var handler in handlers.OfType<Action<T>>())
                {
                    handler(@event);
                }
            }
        }
    }
}
