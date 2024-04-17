using EventBusNet8.Adapter;
using EventBusNet8.Enums;

namespace EventBusNet8.Abstractions;
/// <summary>
/// Defines an interface for an event bus, responsible for managing and dispatching events.
/// </summary>
public interface IEventBus
{
    bool TryAddEvent(string eventNam, object? key, out IEvent value);
    bool TryAddEvent<T>(string eventName, object? key, out IEvent<T> value);
    bool TryListenEvent(string eventName, object? key, Handler handler, EventPhase phase = EventPhase.On);
    bool TryListenEvent<T>(string eventName, object? key, Handler<T> handler, EventPhase phase = EventPhase.On);
    void RemoveEvent(IEvent @event);

    #region Derived Functions
    bool TryAddEvent(string eventName, out IEvent value) => TryAddEvent(eventName, null, out value);
    IEvent AddEvent(string eventName, object? key)
    {
        if (key == EventKey.SomeOneButNullPrefer || key == EventKey.SomeOne || key == EventKey.All)
            throw new ArgumentException("Special Key is not supported here.");
        // ReSharper disable once SuggestVarOrType_SimpleTypes
        return TryAddEvent(eventName, key, out IEvent value) ? value : throw new InvalidOperationException($"Event \"{eventName}\" already exists.");
    }
    IEvent AddEvent(string eventName) => AddEvent(eventName, null);

    bool TryAddEvent<T>(string eventName, out IEvent<T> value) => TryAddEvent(eventName, null, out value);
    IEvent<T> AddEvent<T>(string eventName, object? key)
    {
        if (key == EventKey.SomeOneButNullPrefer || key == EventKey.SomeOne || key == EventKey.All)
            throw new ArgumentException("Special Key is not supported here.");
        return TryAddEvent(eventName, key, out IEvent<T> value) ? value : throw new InvalidOperationException($"Event \"{eventName}\" already exists.");
    }
    IEvent<T> AddEvent<T>(string eventName) => AddEvent<T>(eventName, null!);

    bool TryListenEvent(string eventName, Handler handler, EventPhase phase = EventPhase.On) => TryListenEvent(eventName, EventKey.SomeOneButNullPrefer, handler, phase);
    void ListenEvent(string eventName, object? key, Handler handler, EventPhase phase = EventPhase.On)
    {
        if (!TryListenEvent(eventName, key, handler, phase))
            throw new InvalidOperationException($"Event does not exist.");
    }
    void ListenEvent(string eventName, Handler handler, EventPhase phase = EventPhase.On) => ListenEvent(eventName, EventKey.SomeOneButNullPrefer, handler, phase);

    bool TryListenEvent<T>(string eventName, Handler<T> handler, EventPhase phase = EventPhase.On) => TryListenEvent(eventName, EventKey.SomeOneButNullPrefer, handler, phase);
    void ListenEvent<T>(string eventName, object? key, Handler<T> handler, EventPhase phase = EventPhase.On)
    {
        if (!TryListenEvent(eventName, key, handler, phase))
            throw new InvalidOperationException("Event does not exist or return type is not compatible.");
    }
    void ListenEvent<T>(string eventName, Handler<T> handler, EventPhase phase = EventPhase.On) => ListenEvent(eventName, EventKey.SomeOneButNullPrefer, handler, phase);
    #endregion
}