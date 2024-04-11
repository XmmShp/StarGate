using Shoming.EventBus.Abstractions.Fundamental;

namespace Shoming.EventBus.Abstractions;
public interface IHandler
{
    delegate IResult Functor(IEventParam eventParam);
    IResult Invoke(IEventParam eventParam);
    bool IsAlive { get; }
}

public interface IHandler<out T> : IHandler
{
    new delegate IResult<T> Functor(IEventParam eventParam);
    new IResult<T> Invoke(IEventParam eventParam);
}