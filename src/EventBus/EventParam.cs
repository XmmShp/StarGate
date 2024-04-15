using EventBus.Abstractions;
using EventBus.Enums;

namespace EventBus;
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

    public EventParam(IDictionary<string, object?> values)
    {
        _values = new List<object?>(values.Values);
        _map = values.Keys.Select((key, index) => (key, index)).ToDictionary(pair => pair.key, pair => pair.index);
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

    public bool Remove(int index)
    {
        if (index < 0 || index >= _values.Count)
        {
            return false;
        }
        _values.RemoveAt(index);
        return true;
    }

    public T Get<T>(string name) where T : class => Get<T>(_map[name]);

    public bool TryGet<T>(string name, out T value) where T : class
    {
        if (_map.TryGetValue(name, out var index))
        {
            return TryGet(index, out value);
        }
        value = default!;
        return false;
    }
    public void Push(object value)
    {
        _values.Add(value);
    }

    public void Add(string name, object value)
    {
        _map[name] = _values.Count;
        Push(value);
    }
    public bool TryAdd(string name, object value)
    {
        if (_map.ContainsKey(name))
        {
            return false;
        }
        Add(name, value);
        return true;
    }

    public void SetName(int index, string name)
    {
        _map[name] = index;
    }

    public bool TrySetName(int index, string name)
    {
        if (_map.ContainsKey(name)) return false;
        SetName(index, name);
        return true;
    }

    public bool Remove(string name)
    {
        return _map.Remove(name, out var index) && Remove(index);
    }

    private readonly List<object?> _values;
    private readonly Dictionary<string, int> _map = [];
}