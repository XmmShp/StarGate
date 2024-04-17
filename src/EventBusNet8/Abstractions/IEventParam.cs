using EventBusNet8.Enums;

namespace EventBusNet8.Abstractions;
/// <summary>
/// Defines an interface for event parameters, providing management functionality for event arguments.
/// </summary>
public interface IEventParam
{
    /// <summary>
    /// The status of the event.
    /// </summary>
    EventStatus Status { get; set; }

    /// <summary>
    /// Retrieves an event parameter value by index. This method is a non-generic overload of the generic method.
    /// </summary>
    /// <param name="index">The index position of the parameter.</param>
    /// <returns>The parameter value at the specified index.</returns>
    object? Get(int index) => Get<object?>(index);

    /// <summary>
    /// Retrieves an event parameter value of a specified type by index.
    /// </summary>
    /// <typeparam name="T">The type of the parameter.</typeparam>
    /// <param name="index">The index position of the parameter.</param>
    /// <returns>The parameter value at the specified index, typed as T.</returns>
    T Get<T>(int index);

    /// <summary>
    /// Attempts to retrieve an event parameter value of a specified type by index.
    /// </summary>
    /// <typeparam name="T">The type of the parameter.</typeparam>
    /// <param name="index">The index position of the parameter.</param>
    /// <param name="value">If successful, outputs the parameter value through this parameter.</param>
    /// <returns><see langword="true"/> if the retrieval succeeds; otherwise, <see langword="false"/>.</returns>
    bool TryGet<T>(int index, out T value);

    void Set(int index, object? value);
    bool TrySet(int index, object? value);
    void Set(string name, object? value);
    bool TrySet(string name, object? value);

    /// <summary>
    /// Retrieves an event parameter value by name. This method is a non-generic overload of the generic method.
    /// </summary>
    /// <param name="name">The name of the parameter.</param>
    /// <returns>The parameter value with the specified name.</returns>
    object? Get(string name) => Get<object?>(name);

    /// <summary>
    /// Retrieves an event parameter value of a specified type by name.
    /// </summary>
    /// <typeparam name="T">The type of the parameter.</typeparam>
    /// <param name="name">The name of the parameter.</param>
    /// <returns>The parameter value with the specified name, typed as T.</returns>
    T Get<T>(string name);

    /// <summary>
    /// Attempts to retrieve an event parameter value of a specified type by name.
    /// </summary>
    /// <typeparam name="T">The type of the parameter.</typeparam>
    /// <param name="name">The name of the parameter.</param>
    /// <param name="value">If successful, outputs the parameter value through this parameter.</param>
    /// <returns><see langword="true"/> if the retrieval succeeds; otherwise, <see langword="false"/>.</returns>
    bool TryGet<T>(string name, out T value);
}