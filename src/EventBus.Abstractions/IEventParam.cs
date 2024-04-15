using Shoming.EventBus.Abstractions.Enums;

namespace Shoming.EventBus.Abstractions;
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
    object Get(int index) => Get<object>(index);

    /// <summary>
    /// Retrieves an event parameter value of a specified type by index.
    /// </summary>
    /// <typeparam name="T">The type of the parameter, must be a class type.</typeparam>
    /// <param name="index">The index position of the parameter.</param>
    /// <returns>The parameter value at the specified index, typed as T.</returns>
    T Get<T>(int index) where T : class;

    /// <summary>
    /// Attempts to retrieve an event parameter value of a specified type by index.
    /// </summary>
    /// <typeparam name="T">The type of the parameter, must be a class type.</typeparam>
    /// <param name="index">The index position of the parameter.</param>
    /// <param name="value">If successful, outputs the parameter value through this parameter.</param>
    /// <returns><see langword="true"/> if the retrieval succeeds; otherwise, <see langword="false"/>.</returns>
    bool TryGet<T>(int index, out T value) where T : class;

    /// <summary>
    /// Adds a value to the collection of event parameters.
    /// </summary>
    /// <param name="value">The value to add.</param>
    void Push(object value);

    /// <summary>
    /// Attempts to remove a value from the collection of event parameters by index.
    /// </summary>
    /// <param name="index">The index position of the value to remove.</param>
    /// <returns><see langword="true"/> if the removal succeeds; otherwise, <see langword="false"/>.</returns>
    bool Remove(int index);

    /// <summary>
    /// Retrieves an event parameter value by name. This method is a non-generic overload of the generic method.
    /// </summary>
    /// <param name="name">The name of the parameter.</param>
    /// <returns>The parameter value with the specified name.</returns>
    object Get(string name) => Get<object>(name);

    /// <summary>
    /// Retrieves an event parameter value of a specified type by name.
    /// </summary>
    /// <typeparam name="T">The type of the parameter, must be a class type.</typeparam>
    /// <param name="name">The name of the parameter.</param>
    /// <returns>The parameter value with the specified name, typed as T.</returns>
    T Get<T>(string name) where T : class;

    /// <summary>
    /// Attempts to retrieve an event parameter value of a specified type by name.
    /// </summary>
    /// <typeparam name="T">The type of the parameter, must be a class type.</typeparam>
    /// <param name="name">The name of the parameter.</param>
    /// <param name="value">If successful, outputs the parameter value through this parameter.</param>
    /// <returns><see langword="true"/> if the retrieval succeeds; otherwise, <see langword="false"/>.</returns>
    bool TryGet<T>(string name, out T value) where T : class;

    /// <summary>
    /// Adds a value with a specified name to the collection of event parameters.
    /// </summary>
    /// <param name="name">The name of the parameter.</param>
    /// <param name="value">The value to add.</param>
    void Add(string name, object value);

    /// <summary>
    /// Attempts to add a value with a specified name to the collection of event parameters.
    /// </summary>
    /// <param name="name">The name of the parameter.</param>
    /// <param name="value">The value to add.</param>
    /// <returns><see langword="true"/> if the addition succeeds; otherwise, <see langword="false"/>.</returns>
    bool TryAdd(string name, object value);

    /// <summary>
    /// Sets the name of a parameter at the specified index.
    /// </summary>
    /// <param name="index">The index position of the parameter.</param>
    /// <param name="name">The name to set for the parameter.</param>
    void SetName(int index, string name);

    /// <summary>
    /// Attempts to set the name of a parameter at the specified index.
    /// </summary>
    /// <param name="index">The index position of the parameter.</param>
    /// <param name="name">The name to set for the parameter.</param>
    /// <returns><see langword="true"/> if the name doesn't exist; otherwise, <see langword="false"/>.</returns>
    bool TrySetName(int index, string name);

    /// <summary>
    /// Attempts to remove a parameter from the collection of event parameters by name.
    /// </summary>
    /// <param name="name">The name of the parameter to remove.</param>
    /// <returns><see langword="true"/> if the removal succeeds; otherwise, <see langword="false"/>.</returns>
    bool Remove(string name);
}