namespace EventBus.Abstractions;

public interface IEventBus
{
    IEvent AddEvent(string eventName);
    bool TryAddEvent(string eventName,out IEvent value);
    IEvent<T> AddEvent<T>(string eventName);
    bool TryAddEvent<T>(string eventName, out IEvent<T> value);
    void ListenEvent(string eventName, IHandler handler);
    bool TryListenEvent(string eventName, IHandler handler);
}