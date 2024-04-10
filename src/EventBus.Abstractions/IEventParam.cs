using Shoming.EventBus.Abstractions.Enums;

namespace Shoming.EventBus.Abstractions;
public interface IEventParam
{
    EventStatus Status { get; set; }

    object Get(int index) => Get<object>(index);
    T Get<T>(int index) where T : class;
    bool TryGet<T>(int index, out T value) where T : class;
    void Push(object value);
    bool Remove(int index);

    object Get(string name) => Get<object>(name);
    T Get<T>(string name) where T : class;
    bool TryGet<T>(string name, out T value) where T : class;
    void Add(string name, object value);
    bool TryAdd(string name, object value);
    void SetName(int index, string name);
    bool TrySetName(int index,string name);
    bool Remove(string name);
}