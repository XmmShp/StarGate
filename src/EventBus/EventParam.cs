using Shoming.EventBus.Abstractions;
using Shoming.EventBus.Abstractions.Enums;

namespace Shoming.EventBus;
public class EventParam : IEventParam
{
    public EventParam()
    {
        _values = [];
    }
    public EventParam(IEnumerable<object?> values)
    {
        _values = new List<object?>(values);
    }
    public EventStatus Status { get; set; }

    public T Get<T>(int index) where T : class
    {
        return (T)_values.ElementAt(index)!;
    }

    public bool TryGet<T>(int index, out T value) where T : class
    {
        try
        {
            value = (T)_values.ElementAt(index)!;
            return true;
        }
        catch
        {
            value = default!;
            return false;
        }
    }

    public void Push(object value)
    {
        _values.Add(value);
    }
    private readonly List<object?> _values;
}