namespace Shoming.EventBus.Abstractions;
/// <summary>
/// Defines an interface for an event bus, responsible for managing and dispatching events.
/// </summary>
public interface IEventBus
{
    /// <summary>
    /// Adds an event to the event bus with the specified name.
    /// </summary>
    /// <param name="eventName">The unique name of the event.</param>
    /// <returns>The newly created event instance.</returns>
    IEvent AddEvent(string eventName);

    /// <summary>
    /// Attempts to add an event to the event bus with the specified name.
    /// </summary>
    /// <param name="eventName">The unique name of the event.</param>
    /// <param name="value">When this method returns, contains the added event instance if successful, or null if the event couldn't be added.</param>
    /// <returns><see langword="true"/> if the event with specified name doesn't exist; otherwise, <see langword="false"/>.</returns>
    bool TryAddEvent(string eventName, out IEvent value);

    /// <summary>
    /// Adds a typed event to the event bus with the specified name.
    /// </summary>
    /// <typeparam name="T">The type parameter for the return type.</typeparam>
    /// <param name="eventName">The unique name of the event.</param>
    /// <returns>The newly created event instance.</returns>
    IEvent<T> AddEvent<T>(string eventName);

    /// <summary>
    /// Attempts to add a typed event to the event bus with the specified name.
    /// </summary>
    /// <typeparam name="T">The type parameter for the return type.</typeparam>
    /// <param name="eventName">The unique name of the event.</param>
    /// <param name="value">When this method returns, contains the added event instance if successful, or null if the event couldn't be added.</param>
    /// <returns><see langword="true"/> if the event with specified name doesn't exist; otherwise, <see langword="false"/>.</returns>
    bool TryAddEvent<T>(string eventName, out IEvent<T> value);

    /// <summary>
    /// Registers a handler to listen for events with the specified name.
    /// </summary>
    /// <param name="eventName">The name of the event to which the handler should subscribe.</param>
    /// <param name="handler">The handler instance that will process events when they are dispatched.</param>
    void ListenEvent(string eventName, IHandler handler);

    /// <summary>
    /// Attempts to register a handler to listen for events with the specified name.
    /// </summary>
    /// <param name="eventName">The name of the event to which the handler should subscribe.</param>
    /// <param name="handler">The handler instance that will process events when they are dispatched.</param>
    /// <returns><see langword="true"/> if the event was existed; otherwise, <see langword="false"/>.</returns>
    bool TryListenEvent(string eventName, IHandler handler);
}