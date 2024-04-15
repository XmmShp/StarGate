using Shoming.EventBus.Abstractions.Enums;
namespace Shoming.EventBus.Abstractions;
/// <summary>
/// Represents an interface for the result of an event processing operation, providing access to the status of the operation.
/// </summary>
public interface IResult
{
    /// <summary>
    /// Gets the status of the event processing operation, indicating whether it was continued, interrupted, or others.
    /// </summary>
    /// <value>A <see cref="EventStatus"/> enumeration value representing the status of the event processing operation.</value>
    EventStatus Status { get; }
}

/// <summary>
/// Provides a generic variant of the <see cref="IResult"/> interface, allowing for typed return values from event processing operations.
/// </summary>
/// <typeparam name="T">The type of the return value.</typeparam>
public interface IResult<out T> : IResult
{
    /// <summary>
    /// Gets the typed return value resulting from the event processing operation, if any.
    /// </summary>
    /// <value>The typed return value, or <see langword="null"/> if no value was returned or the operation failed.</value>
    T? Return { get; }
}