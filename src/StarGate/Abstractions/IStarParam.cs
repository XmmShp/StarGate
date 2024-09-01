using StarGate.Enums;
using System;
using System.Collections;

namespace StarGate.Abstractions
{
    /// <summary>
    /// Defines an interface for event parameters, providing management functionality for event arguments.
    /// </summary>
    public interface IStarParam : IEnumerable
    {
        #region properties

        /// <summary>
        /// The status of the event.
        /// </summary>
        StarStatus Status { get; set; }

        #endregion

        #region interfaces

        /// <summary>
        /// Retrieves an event parameter value by index. This method is a non-generic overload of the generic method.
        /// </summary>
        /// <param name="index">The index position of the parameter.</param>
        /// <returns>The parameter value at the specified index.</returns>
        object? Get(int index);

        /// <summary>
        /// Retrieves an event parameter value by name. This method is a non-generic overload of the generic method.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <returns>The parameter value with the specified name.</returns>
        object? Get(string name);

        void Set(int index, object? value);
        void Set(string name, object? value);

        #endregion

        #region derived functions

        /// <summary>
        /// Retrieves an event parameter value of a specified type by index.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="index">The index position of the parameter.</param>
        /// <returns>The parameter value at the specified index, typed as T.</returns>
        T Get<T>(int index)
        {
            if (Get(index) is T ret)
            {
                return ret;
            }

            throw new InvalidCastException($"Param in index: {index} is not {typeof(T).Name}");
        }

        /// <summary>
        /// Retrieves an event parameter value of a specified type by name.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="name">The name of the parameter.</param>
        /// <returns>The parameter value with the specified name, typed as T.</returns>
        T Get<T>(string name)
        {
            if (Get(name) is T ret)
            {
                return ret;
            }

            throw new InvalidCastException($"Param with name: {name} is not {typeof(T).Name}");
        }

        /// <summary>
        /// Attempts to retrieve an event parameter value of a specified type by index.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="index">The index position of the parameter.</param>
        /// <param name="value">If successful, outputs the parameter value through this parameter.</param>
        /// <returns><see langword="true"/> if the retrieval succeeds; otherwise, <see langword="false"/>.</returns>
        bool TryGet<T>(int index, out T value)
        {
            try
            {
                value = Get<T>(index);
                return true;
            }
            catch
            {
                value = default!;
                return false;
            }
        }

        /// <summary>
        /// Attempts to retrieve an event parameter value of a specified type by name.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">If successful, outputs the parameter value through this parameter.</param>
        /// <returns><see langword="true"/> if the retrieval succeeds; otherwise, <see langword="false"/>.</returns>
        bool TryGet<T>(string name, out T value)
        {
            try
            {
                value = Get<T>(name);
                return true;
            }
            catch
            {
                value = default!;
                return false;
            }
        }


        bool TrySet(int index, object? value)
        {
            try
            {
                Set(index, value);
                return true;
            }
            catch
            {
                return false;
            }
        }


        bool TrySet(string name, object? value)
        {
            try
            {
                Set(name, value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

    }
}
