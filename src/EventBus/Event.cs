using Shoming.EventBus.Abstractions;
using Shoming.EventBus.Abstractions.Enums;

namespace Shoming.EventBus;
public class Event : IEvent
{
    public IResult Invoke(IEventParam param)
    {
        IResult res = null!;
        foreach (var handler in _handlers.TakeWhile(_ => param.Status != EventStatus.Interrupted))
        {
            res = handler.Invoke(param);
        }
        return res;
    }

    public void ListenEvent(IHandler handler)
    {
        _handlers.Add(handler);
    }
    private readonly List<IHandler> _handlers = [];
}
public class Event<T> : Event, IEvent<T>
{
    public new IResult<T> Invoke(IEventParam param)
    {
        base.Invoke(param);
        IResult<T> res = null!;
        foreach (var handler in _handlers.TakeWhile(_ => param.Status != EventStatus.Interrupted))
        {
            res = handler.Invoke(param);
        }
        return res;
    }

    public void ListenEvent(IHandler<T> handler)
    {
        _handlers.Add(handler);
    }

    private readonly List<IHandler<T>> _handlers = [];
}