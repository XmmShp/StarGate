using Shoming.EventBus.Abstractions.Fundamental;

namespace Shoming.EventBus.Abstractions;
public interface IEvent
{
    IResult Invoke(IEventParam handler);
    void ListenEvent(IHandler handler);
}

public interface IEvent<T> : IEvent
{
    new IResult<T> Invoke(IEventParam param);
    void ListenEvent(IHandler<T> handler);
}