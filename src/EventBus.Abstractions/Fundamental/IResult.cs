using EventBus.Abstractions.Enums;
namespace EventBus.Abstractions.Fundamental;

public interface IResult
{
    EventStatus Status { get; }
}

public interface IResult<out T> : IResult
{
    T? Return { get; }
}