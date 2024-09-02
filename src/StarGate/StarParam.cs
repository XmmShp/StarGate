using StarGate.Abstractions;
using StarGate.Enums;
using System;
using System.Collections;
using System.Collections.Generic;

namespace StarGate
{
    public class StarParam : IStarParam
    {
        public StarParam() => _values = new List<object?>();

        public IEnumerator GetEnumerator() => _values.GetEnumerator();

        public StarParam(IEnumerable? values)
        {
            _values = new List<object?>();
            if (values is null) return;
            foreach (var value in values)
            {
                Push(value);
            }
        }

        public StarParam(IDictionary values)
        {
            _values = new List<object?>();
            foreach (DictionaryEntry entry in values)
            {
                if (entry.Key is not string key)
                {
                    throw new ArgumentException("Key must be a string", nameof(values));
                }
                Add(key, entry.Value);
            }
        }

        public StarStatus Status { get; set; }

        public bool Remove(int index)
        {
            if (index < 0 || index >= _values.Count)
            {
                return false;
            }
            _values.RemoveAt(index);
            return true;
        }

        public object? Get(int index) => _values[index];

        public object? Get(string name) => _values[_map[name]];

        public void Set(int index, object? value) => _values[index] = value;

        public void Set(string name, object? value) => Set(_map[name], value);

        public void Push(object? value) => _values.Add(value);

        public void Add(string name, object? value)
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

        public void SetName(int index, string name) => _map[name] = index;

        public bool TrySetName(int index, string name)
        {
            if (_map.ContainsKey(name)) return false;
            SetName(index, name);
            return true;
        }

        public bool Remove(string name) => _map.Remove(name, out var index) && Remove(index);

        private readonly List<object?> _values;
        private readonly Dictionary<string, int> _map = new();
    }
}

