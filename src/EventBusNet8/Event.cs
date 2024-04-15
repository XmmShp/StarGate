using EventBusNet8.Abstractions;
using EventBusNet8.Enums;

namespace EventBusNet8;

internal class Event(string name, object key, IEventBus eventBus) : IEvent
{
    public IResult Invoke(IEventParam param)
    {
        var result = InvokePhase(param, EventPhase.Pre);
        if (result.Status != EventStatus.Interrupted)
            result = InvokePhase(param, EventPhase.Peri);
        if (result.Status != EventStatus.Interrupted)
            result = InvokePhase(param, EventPhase.Post);
        return result;
    }

    public void Unload(IEventParam param)
    {
        EventBus.RemoveEvent(this);
        foreach (var handler in _handlers[EventPhase.Unload])
        {
            handler.Invoke(param);
        }
    }

    protected IResult InvokePhase(IEventParam param, EventPhase phase)
    {
        IResult res = new Result(EventStatus.Continued);
        foreach (var handler in _handlers[phase].TakeWhile(_ => param.Status != EventStatus.Interrupted))
        {
            res = handler.Invoke(param);
        }
        return res;
    }

    public void ListenEvent(IHandler handler, EventPhase phase)
    {
        _handlers[phase].Add(handler);
    }

    private readonly Dictionary<EventPhase, List<IHandler>> _handlers = new()
    {
        { EventPhase.Pre ,[]},
        { EventPhase.Peri ,[]},
        { EventPhase.Post ,[]},
        { EventPhase.Unload ,[]}
    };

    public string Name { get; } = name;
    public object Key { get; } = key;
    protected IEventBus EventBus { get; } = eventBus;
}
internal class Event<T>(string name, object key, IEventBus eventBus) : Event(name, key, eventBus), IEvent<T>
{
    public new IResult<T> Invoke(IEventParam param)
    {
        var result = InvokePhase(param, EventPhase.Pre);
        if (result.Status != EventStatus.Interrupted)
            result = InvokePhase(param, EventPhase.Peri);
        if (result.Status != EventStatus.Interrupted)
            result = InvokePhase(param, EventPhase.Post);
        return result;
    }

    protected new IResult<T> InvokePhase(IEventParam param, EventPhase phase)
    {
        IResult<T> res = new Result<T>(default, base.InvokePhase(param, phase).Status);
        foreach (var handler in _typedHandlers[phase].TakeWhile(_ => param.Status != EventStatus.Interrupted))
        {
            res = handler.Invoke(param);
        }
        return res;
    }

    public void ListenEvent(IHandler<T> handler, EventPhase phase)
    {
        if (phase is EventPhase.Unload)
            throw new InvalidOperationException("Unload phase is not supported for typed event.");
        _typedHandlers[phase].Add(handler);
    }

    private readonly Dictionary<EventPhase, List<IHandler<T>>> _typedHandlers = new()
    {
        { EventPhase.Pre ,[]},
        { EventPhase.Peri ,[]},
        { EventPhase.Post ,[]}
    };
}