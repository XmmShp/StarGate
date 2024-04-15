using EventBus.Enums;

namespace EventBus.Abstractions;

internal class SomeOne;
internal class All;
internal class SomeOneButNullPrefer;

/// <summary>
/// Defines an interface for an event bus, responsible for managing and dispatching events.
/// </summary>
public interface IEventBus
{
    public static object SomeOne { get; } = new SomeOne();
    public static object All { get; } = new All();
    public static object SomeOneButNullPrefer { get; } = new SomeOneButNullPrefer();

    bool TryAddEvent(string eventNam, object? key, out IEvent value);
    bool TryAddEvent<T>(string eventName, object? key, out IEvent<T> value);
    bool TryListenEvent(string eventName, object? key, IHandler handler, EventPhase phase = EventPhase.Peri);
    bool TryListenEvent<T>(string eventName, object? key, IHandler<T> handler, EventPhase phase = EventPhase.Peri);
    void RemoveEvent(string eventName, object? key, bool doUnload = true);

    #region Derived Functions
    bool TryAddEvent(string eventName, out IEvent value) => TryAddEvent(eventName, null, out value);
    IEvent AddEvent(string eventName, object? key)
    {
        if (key == SomeOneButNullPrefer || key == SomeOne || key == All)
            throw new ArgumentException("Special Key is not supported here.");
        // ReSharper disable once SuggestVarOrType_SimpleTypes
        return TryAddEvent(eventName, key, out IEvent value) ? value : throw new InvalidOperationException($"Event \"{eventName}\" already exists.");
    }
    IEvent AddEvent(string eventName) => AddEvent(eventName, null);

    bool TryAddEvent<T>(string eventName, out IEvent<T> value) => TryAddEvent(eventName, null, out value);
    IEvent<T> AddEvent<T>(string eventName, object? key)
    {
        if (key == SomeOneButNullPrefer || key == SomeOne || key == All)
            throw new ArgumentException("Special Key is not supported here.");
        return TryAddEvent(eventName, key, out IEvent<T> value) ? value : throw new InvalidOperationException($"Event \"{eventName}\" already exists.");
    }
    IEvent<T> AddEvent<T>(string eventName) => AddEvent<T>(eventName, null!);

    bool TryListenEvent(string eventName, IHandler handler, EventPhase phase = EventPhase.Peri) => TryListenEvent(eventName, SomeOneButNullPrefer, handler, phase);
    void ListenEvent(string eventName, object? key, IHandler handler, EventPhase phase = EventPhase.Peri)
    {
        if (!TryListenEvent(eventName, key, handler, phase))
            throw new InvalidOperationException($"Event does not exist.");
    }
    void ListenEvent(string eventName, IHandler handler, EventPhase phase = EventPhase.Peri) => ListenEvent(eventName, SomeOneButNullPrefer, handler, phase);

    bool TryListenEvent<T>(string eventName, IHandler<T> handler, EventPhase phase = EventPhase.Peri) => TryListenEvent(eventName, SomeOneButNullPrefer, handler, phase);
    void ListenEvent<T>(string eventName, object? key, IHandler<T> handler, EventPhase phase = EventPhase.Peri)
    {
        if (!TryListenEvent(eventName, key, handler, phase))
            throw new InvalidOperationException("Event does not exist or return type is not compatible.");
    }
    void ListenEvent<T>(string eventName, IHandler<T> handler, EventPhase phase = EventPhase.Peri) => ListenEvent(eventName, SomeOneButNullPrefer, handler, phase);

    void RemoveEvent(string eventName) => RemoveEvent(eventName, All);
    void RemoveEvent(IEvent @event) => RemoveEvent(@event.Name, @event.Key);
    #endregion
}