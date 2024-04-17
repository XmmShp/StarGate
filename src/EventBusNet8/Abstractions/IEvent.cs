using EventBusNet8.Adapter;
using EventBusNet8.Enums;
using EventBusNet8.Fundamental;

namespace EventBusNet8.Abstractions;
/// <summary>
/// Represents an interface for an event, defining methods for invoking an event handler and registering listeners.
/// </summary>
public interface IEvent
{
    EventResult Invoke(EventParam param);
    void Unload(EventParam param);
    Task<EventResult> InvokeAsync(EventParam param);

    /// <summary>
    /// Registers an event listener to receive notifications when the event is triggered.
    /// </summary>
    /// <param name="handler">The event listener instance that will be notified when the event occurs.</param>
    /// <param name="phase">The phase of the event to listen for, pre,On or post.</param>
    void ListenEvent(Handler handler, EventPhase phase);
    string Name { get; }
    object Key { get; }
}

/// <summary>
/// Provides a generic version of the <see cref="IEvent"/> interface, allowing for typed event results.
/// </summary>
/// <typeparam name="T">The type of the event result.</typeparam>
public interface IEvent<T> : IEvent
{
    /// <summary>
    /// Invokes the event with the provided event parameters, passing them to the subscribed handler and returning a typed result.
    /// </summary>
    /// <param name="param">The event parameters to be passed to the handler.</param>
    /// <returns>The typed result of the event invocation, represented as a <see cref="EventResult{T}"/> object.</returns>
    new EventResult<T> Invoke(EventParam param);
    new Task<EventResult<T>> InvokeAsync(EventParam param);
    /// <summary>
    /// Registers a typed event listener to receive notifications when the event is triggered.
    /// </summary>
    /// <param name="handler">The typed event listener instance that will be notified when the event occurs.</param>
    /// <param name="phase">The phase of the event to listen for, pre,On or post.</param>
    void ListenEvent(Handler<T> handler, EventPhase phase);
}