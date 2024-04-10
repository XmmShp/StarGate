using Shoming.EventBus.Abstractions;
namespace Shoming.EventBus;
public class EventBus : IEventBus
{
    public IEvent AddEvent(string eventName) => _events[eventName] = new Event();

    public bool TryAddEvent(string eventName, out IEvent value)
    {
        if (_events.ContainsKey(eventName))
        {
            value = default!;
            return false;
        }

        value = _events[eventName] = new Event();
        return true;
    }

    public IEvent<T> AddEvent<T>(string eventName) => (IEvent<T>)(_events[eventName] = new Event<T>());

    public bool TryAddEvent<T>(string eventName, out IEvent<T> value)
    {
        if (_events.ContainsKey(eventName))
        {
            value = default!;
            return false;
        }

        value = (IEvent<T>)(_events[eventName] = new Event<T>());
        return true;
    }

    public void ListenEvent(string eventName, IHandler handler) => _events[eventName].ListenEvent(handler);

    public bool TryListenEvent(string eventName, IHandler handler)
    {
        if (!_events.TryGetValue(eventName, out var value)) return false;
        value.ListenEvent(handler);
        return true;
    }

    private readonly Dictionary<string, IEvent> _events = [];
}