using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using EventBusNet8.Abstractions;
using EventBusNet8.Adapter;
using EventBusNet8.Enums;
using EventBusNet8.Fundamental;

namespace EventBusNet8;

internal class Event(string name, object key, IEventBus eventBus) : IEvent
{
    public EventResult Invoke(EventParam param)
    {
        var result = InvokePhase(param, EventPhase.Pre);
        if ((result.Status & EventStatus.Interrupted) != EventStatus.Interrupted)
            result = InvokePhase(param, EventPhase.On);
        if ((result.Status & EventStatus.Interrupted) != EventStatus.Interrupted)
            InvokePhase(param, EventPhase.Post);
        return result;
    }

    public void Unload(EventParam param)
    {
        EventBus.RemoveEvent(this);
        foreach (var handler in Handlers[EventPhase.Unload])
        {
            handler.Invoke(param);
        }
    }

    protected EventResult InvokePhase(EventParam param, EventPhase phase)
    {
        foreach (var handler in Handlers[phase].TakeWhile(_ => (param.Status & EventStatus.Interrupted) != EventStatus.Interrupted))
        {
            param.Status |= handler.Invoke(param).Status;
        }
        return param.Status;
    }

    public void ListenEvent(Handler handler, EventPhase phase)
    {
        Handlers[phase].Add(handler);
    }

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
    protected IEventBus EventBus { get; } = eventBus;
}
internal class Event<T>(string name, object key, IEventBus eventBus) : Event(name, key, eventBus), IEvent<T>
{
    public new EventResult<T> Invoke(EventParam param)
    {
        var result = InvokePhase(param, EventPhase.Pre);
        if ((result.Status & EventStatus.Interrupted) != EventStatus.Interrupted)
            result = InvokePhase(param, EventPhase.On);
        if ((result.Status & EventStatus.Interrupted) != EventStatus.Interrupted)
            InvokePhase(param, EventPhase.Post);
        return result;
    }

    public new async Task<EventResult<T>> InvokeAsync(EventParam param) => await Task.Run(() => Invoke(param));

    protected new EventResult<T> InvokePhase(EventParam param, EventPhase phase)
    {
        base.InvokePhase(param, phase);
        T? res = default;
        foreach (var handler in _typedHandlers[phase].TakeWhile(_ => (param.Status & EventStatus.Interrupted) != EventStatus.Interrupted))
        {
            var temp = handler.Invoke(param);
            param.Status |= temp.Status;
            res = temp.Return;
        }
        return (res, param.Status);
    }

    public void ListenEvent(Handler<T> handler, EventPhase phase)
    {
        if (phase is EventPhase.Unload)
            Handlers[phase].Add(handler);
        else
            _typedHandlers[phase].Add(handler);
    }

    private readonly Dictionary<EventPhase, List<Handler<T>>> _typedHandlers = new()
    {
        { EventPhase.Pre ,[]},
        { EventPhase.On ,[]},
        { EventPhase.Post ,[]}
    };
}