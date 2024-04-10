using Shoming.EventBus.Abstractions.Enums;
namespace Shoming.EventBus.Abstractions.Fundamental;
public interface IResult
{
    EventStatus Status { get; }
}

public interface IResult<out T> : IResult
{
    T? Return { get; }
}