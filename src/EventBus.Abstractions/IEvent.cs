namespace Shoming.EventBus.Abstractions;
/// <summary>
/// Represents an interface for an event, defining methods for invoking an event handler and registering listeners.
/// </summary>
public interface IEvent
{
    /// <summary>
    /// Invokes the event with the provided event parameters, passing them to the subscribed handler.
    /// </summary>
    /// <param name="handler">The event parameters to be passed to the handler.</param>
    /// <returns>The result of the event invocation, represented as a <see cref="IResult"/> object.</returns>
    IResult Invoke(IEventParam handler);

    /// <summary>
    /// Registers an event listener to receive notifications when the event is triggered.
    /// </summary>
    /// <param name="handler">The event listener instance that will be notified when the event occurs.</param>
    void ListenEvent(IHandler handler);
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
    /// <returns>The typed result of the event invocation, represented as a <see cref="IResult{T}"/> object.</returns>
    new IResult<T> Invoke(IEventParam param);

    /// <summary>
    /// Registers a typed event listener to receive notifications when the event is triggered.
    /// </summary>
    /// <param name="handler">The typed event listener instance that will be notified when the event occurs.</param>
    void ListenEvent(IHandler<T> handler);
}