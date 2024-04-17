namespace EventBusNet8.Enums;

[Flags]
public enum EventStatus
{
    Continued = 1,
    ParamChanged = 2,
    Interrupted = 4
}