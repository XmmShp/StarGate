using EventBus.Abstractions.Enums;

namespace EventBus.Abstractions;

public interface IEventParam
{
    EventStatus Status { get; set; }

    object Get(int index) => Get<object>(index);
    T Get<T>(int index) where T : class;
    bool TryGet<T>(int index, out T value) where T : class;
    void Push(object value);
}