using System.Collections.Concurrent;

namespace MyLibrary.Server.Events
{
    public class EventBus
    {
        private readonly ConcurrentDictionary<Type, List<Delegate>> _handlers = new();

        public void Subscribe<T>(Action<T> handler)
        {
            if (!_handlers.ContainsKey(typeof(T)))
            {
                _handlers[typeof(T)] = new List<Delegate>();
                Console.WriteLine($"[EventBus] First-time subscription for event: {typeof(T).Name}");
            }
            _handlers[typeof(T)].Add(handler);
            Console.WriteLine($"[EventBus] Subscribed to event: {typeof(T).Name}. Total handlers: {_handlers[typeof(T)].Count}");
        }

        public void Publish<T>(T @event)
        {
            Console.WriteLine($"[EventBus] Publishing event: {typeof(T).Name}");
            if (_handlers.TryGetValue(typeof(T), out var handlers))
            {
                foreach (var handler in handlers.OfType<Action<T>>())
                {
                    Console.WriteLine($"[EventBus] Invoking handler for {typeof(T).Name}");
                    handler(@event);
                }
            }
            else
            {
                Console.WriteLine($"[EventBus] No handlers found for event: {typeof(T).Name}");
            }
        }
    }
}
