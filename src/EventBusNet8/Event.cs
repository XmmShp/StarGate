using EventBusNet8.Abstractions;
using EventBusNet8.Adapter;
using EventBusNet8.Enums;
using EventBusNet8.Fundamental;

namespace EventBusNet8;

internal class Event(string name, object key, IEventBus eventBus) : IEvent
{
    public EventResult Invoke(EventParam param)
    {
        InvokePhase(param, EventPhase.Pre);
        if (!param.Status.IsInterrupt())
            param.Status |= InvokePhase(param, EventPhase.On);
        if (!param.Status.IsInterrupt())
            InvokePhase(param, EventPhase.Post);
        return param.Status;
    }

    public void Unload(EventParam param)
    {
        eventBus.RemoveEvent(this);
        foreach (var handler in Handlers[EventPhase.Unload])
        {
            handler.Invoke(param);
        }
    }

    protected EventResult InvokePhase(EventParam param, EventPhase phase)
    {
        foreach (var handler in Handlers[phase].TakeWhile(_ => !param.Status.IsInterrupt()))
        {
            param.Status |= handler.Invoke(param).Status;
        }
        return param.Status;
    }

    public void ListenEvent(Functor handler, EventPhase phase) => Handlers[phase].Add(new Handler(handler));

    protected readonly Dictionary<EventPhase, List<Handler>> Handlers = new()
    {
        { EventPhase.Pre ,[]},
        { EventPhase.On ,[]},
        { EventPhase.Post ,[]},
        { EventPhase.Unload ,[]}
    };

    public async Task<EventResult> InvokeAsync(EventParam param) => await Task.Run(() => Invoke(param));

    public string Name { get; } = name;
    public object Key { get; } = key;
}
internal class Event<T>(string name, object key, IEventBus eventBus) : Event(name, key, eventBus), IEvent<T>
{
    public new EventResult<T> Invoke(EventParam param)
    {
        T? result = default;
        InvokePhase(param, EventPhase.Pre);
        if (!param.Status.IsInterrupt())
        {
            param.Status |= InvokePhase(param, EventPhase.On);
            foreach (var handler in _typedHandlers[EventPhase.On].TakeWhile(_ => !param.Status.IsInterrupt()))
            {
                var res = handler.Invoke(param);
                param.Status |= res;
                if (param.Status.IsSolve())
                {
                    result = res.Return;
                }
            }
        }
        if (!param.Status.IsInterrupt())
            InvokePhase(param, EventPhase.Post);
        return (result, param.Status);
    }

    public new async Task<EventResult<T>> InvokeAsync(EventParam param) => await Task.Run(() => Invoke(param));

    public void ListenEvent(Functor<T> handler, EventPhase phase)
    {
        if (phase is EventPhase.On)
            _typedHandlers[phase].Add(new Handler<T>(handler));
        else
            Handlers[phase].Add(new Handler<T>(handler));
    }

    private readonly Dictionary<EventPhase, List<Handler<T>>> _typedHandlers = new()
    {
        { EventPhase.On ,[]},
    };
}