using System.Collections;
using EventBusNet8.Adapter;
using EventBusNet8.Enums;
using EventBusNet8.Fundamental;

namespace EventBusNet8.Abstractions;
public interface IEvent
{
    string Name { get; }
    object Key { get; }
    EventResult Invoke(EventParam param);
    Task<EventResult> InvokeAsync(EventParam param);
    void ListenEvent(Functor handler, EventPhase phase);
    void Unload(EventParam param);

    #region Derived Functions
    EventResult Invoke(IEnumerable param) => Invoke(new EventParam(param));
    EventResult Invoke(IDictionary param) => Invoke(new EventParam(param));
    EventResult Invoke(params object?[] param) => Invoke(new EventParam(param));
    Task<EventResult> InvokeAsync(IEnumerable param) => InvokeAsync(new EventParam(param));
    Task<EventResult> InvokeAsync(IDictionary param) => InvokeAsync(new EventParam(param));
    Task<EventResult> InvokeAsync(params object?[] param) => InvokeAsync(new EventParam(param));
    #endregion
}

public interface IEvent<T> : IEvent
{
    new EventResult<T> Invoke(EventParam param);
    new Task<EventResult<T>> InvokeAsync(EventParam param);
    void ListenEvent(Functor<T> handler, EventPhase phase);

    #region Derived Functions
    new EventResult<T> Invoke(IEnumerable param) => Invoke(new EventParam(param));
    new EventResult<T> Invoke(IDictionary param) => Invoke(new EventParam(param));
    new EventResult<T> Invoke(params object?[] param) => Invoke(new EventParam(param));
    new Task<EventResult<T>> InvokeAsync(IEnumerable param) => InvokeAsync(new EventParam(param));
    new Task<EventResult<T>> InvokeAsync(IDictionary param) => InvokeAsync(new EventParam(param));
    new Task<EventResult<T>> InvokeAsync(params object?[] param) => InvokeAsync(new EventParam(param));
    #endregion

}