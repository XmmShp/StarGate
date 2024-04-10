using Shoming.EventBus.Abstractions.Fundamental;

namespace Shoming.EventBus.Abstractions;
public interface IHandler
{
    delegate void Functor(IEventParam eventParam);
    IResult Invoke(IEventParam eventParam);
    bool IsAlive { get; }
}

public interface IHandler<out T> : IHandler
{
    new delegate T Functor(IEventParam eventParam);
    new IResult<T> Invoke(IEventParam eventParam);
}