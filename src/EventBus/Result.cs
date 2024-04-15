using Shoming.EventBus.Abstractions;
using Shoming.EventBus.Abstractions.Enums;

namespace Shoming.EventBus;
public class Result(EventStatus status) : IResult
{
    public EventStatus Status { get; } = status;
}

public class Result<T>(T? @return, EventStatus status) : Result(status), IResult<T>
{
    public T? Return { get; } = @return;
}