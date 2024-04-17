using EventBusNet8.Enums;

namespace EventBusNet8.Fundamental;
public class EventResult(EventStatus status)
{
    public EventStatus Status { get; } = status;
    public static implicit operator EventResult(EventStatus status) => new(status);
    public static implicit operator EventStatus(EventResult result) => result.Status;
}

public class EventResult<T>(T? @return, EventStatus status) : EventResult(status)
{
    public T? Return { get; } = @return;
    public static implicit operator EventResult<T>((T? @return, EventStatus status) tuple) => new(tuple.@return, tuple.status);
    public static implicit operator EventResult<T>((EventStatus status, T? @return) tuple) => new(tuple.@return, tuple.status);
    public static implicit operator EventResult<T>(T? @return) => new(@return, EventStatus.Interrupted);
    public static implicit operator EventResult<T>(EventStatus status) => new(default, status);
    public static implicit operator T?(EventResult<T> eventResult) => eventResult.Return;
    public static implicit operator EventStatus(EventResult<T> result) => result.Status; 
}