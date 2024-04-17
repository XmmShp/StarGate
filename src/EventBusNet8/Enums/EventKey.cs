namespace EventBusNet8.Enums;

internal class SomeOne;
internal class All;
internal class SomeOneButNullPrefer;
public static class EventKey
{
    public static object SomeOne { get; } = new SomeOne();
    public static object All { get; } = new All();
    public static object SomeOneButNullPrefer { get; } = new SomeOneButNullPrefer();
}