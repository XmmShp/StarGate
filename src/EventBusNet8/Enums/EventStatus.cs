namespace EventBusNet8.Enums;

[Flags]
public enum EventStatus
{
    Continue = 1,//Available for EventPhase.Pre, EventPhase.On and EventPhase.Post
    ParamChange = 2,// Available for EventPhase.Pre and EventPhase.On
    Interrupt = 4,// Available for EventPhase.Pre and EventPhase.On
    Solve = 8,// Available only for EventPhase.On
    Final = Interrupt | Solve
}

public static class EventStatusChecker
{
    public static bool IsContinue(this EventStatus status) => status.HasFlag(EventStatus.Continue);
    public static bool IsParamChange(this EventStatus status) => status.HasFlag(EventStatus.ParamChange);
    public static bool IsInterrupt(this EventStatus status) => status.HasFlag(EventStatus.Interrupt);
    public static bool IsSolve(this EventStatus status) => status.HasFlag(EventStatus.Solve);
}