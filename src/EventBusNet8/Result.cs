using EventBusNet8.Abstractions;
using EventBusNet8.Enums;

namespace EventBusNet8;
public class Result(EventStatus status) : IResult
{
    public EventStatus Status { get; } = status;
}

public class Result<T>(T? @return, EventStatus status) : Result(status), IResult<T>
{
    public T? Return { get; } = @return;
}