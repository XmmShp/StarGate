namespace EventBusNet8.Abstractions;
/// <summary>
/// Defines an interface for an event handling component, providing a delegate for processing events and a method to invoke the handler.
/// </summary>
public interface IHandler
{
    /// <summary>
    /// A delegate representing a function that takes a <see cref="IEventParam"/> and returns a <see cref="IResult"/>.
    /// This delegate is used to define the event processing logic for the handler.
    /// </summary>
    /// <param name="eventParam">The event parameters to be processed by the functor.</param>
    /// <returns>A <see cref="IResult"/> object representing the outcome of processing the event.</returns>
    delegate IResult Functor(IEventParam eventParam);

    /// <summary>
    /// Invokes the event handling logic with the provided event parameters.
    /// </summary>
    /// <param name="eventParam">The event parameters to be passed to the handler.</param>
    /// <returns>A <see cref="IResult"/> object representing the outcome of processing the event.</returns>
    IResult Invoke(IEventParam eventParam);

    /// <summary>
    /// Gets a value indicating whether the handler is currently active or alive.
    /// </summary>
    /// <value><see langword="true"/> if the handler is alive; otherwise, <see langword="false"/>.</value>
    bool IsAlive { get; }
}

/// <summary>
/// Provides a generic variant of the <see cref="IHandler"/> interface, allowing for typed event results.
/// </summary>
/// <typeparam name="T">The type of the event result.</typeparam>
public interface IHandler<out T> : IHandler
{
    /// <summary>
    /// A generic delegate representing a function that takes a <see cref="IEventParam"/> and returns a <see cref="IResult{T}"/>.
    /// This delegate is used to define the event processing logic for the handler, with a typed result.
    /// </summary>
    /// <param name="eventParam">The event parameters to be processed by the functor.</param>
    /// <returns>A <see cref="IResult{T}"/> object representing the typed outcome of processing the event.</returns>
    new delegate IResult<T> Functor(IEventParam eventParam);

    /// <summary>
    /// Invokes the event handling logic with the provided event parameters, returning a typed result.
    /// </summary>
    /// <param name="eventParam">The event parameters to be passed to the handler.</param>
    /// <returns>A <see cref="IResult{T}"/> object representing the typed outcome of processing the event.</returns>
    new IResult<T> Invoke(IEventParam eventParam);
}