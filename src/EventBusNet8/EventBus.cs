using EventBusNet8.Abstractions;
using EventBusNet8.Adapter;
using EventBusNet8.Enums;

namespace EventBusNet8;

internal class NullKey;
public class EventBus : IEventBus
{
    private readonly object _nullKey = new NullKey();

    public void RemoveEvent(IEvent @event) => RemoveEvent(@event.Name, @event.Key, [], false);
    public void RemoveEvent(string eventName, EventParam param) => RemoveEvent(eventName, EventKey.All, param, true);
    public void RemoveEvent(string eventName) => RemoveEvent(eventName, []);

    public bool TryAddEvent(string eventName, object? key, out IEvent value)
    {
        if (key == EventKey.All || key == EventKey.SomeOne || key == EventKey.SomeOneButNullPrefer)
        {
            value = default!;
            return false;
        }
        key ??= _nullKey;
        if (_events.TryGetValue(eventName, out var refs) && refs.TryGetValue(key, out var cur) && cur.TryGetTarget(out _))
        {
            value = default!;
            return false;
        }
        value = new Event(eventName, key, this);
        _events.TryAdd(eventName, []);
        _events[eventName][key] = new WeakReference<IEvent>(value);
        return true;
    }

    public bool TryAddEvent<T>(string eventName, object? key, out IEvent<T> value)
    {
        if (key == EventKey.All || key == EventKey.SomeOne || key == EventKey.SomeOneButNullPrefer)
        {
            value = default!;
            return false;
        }
        key ??= _nullKey;
        if (_events.TryGetValue(eventName, out var refs) && refs.TryGetValue(key, out var cur) && cur.TryGetTarget(out _))
        {
            value = default!;
            return false;
        }
        value = new Event<T>(eventName, key, this);
        _events.TryAdd(eventName, []);
        _events[eventName][key] = new WeakReference<IEvent>(value);
        return true;
    }

    public bool TryListenEvent(string eventName, object? key, Functor handler, EventPhase phase)
    {
        if (!_events.TryGetValue(eventName, out var events)) return false;//no event

        if (key == EventKey.SomeOneButNullPrefer)
        {
            // ReSharper disable once InvertIf
            if (events.TryGetValue(_nullKey, out var nullEvent))//try to get event with null-key
            {
                if (nullEvent.TryGetTarget(out var nullTarget))
                {
                    nullTarget.ListenEvent(handler, phase);
                    return true;
                }
                events.Remove(_nullKey);
            }
            return ListenSomeOne();
        }

        if (key == EventKey.All)
        {
            var flag = false;
            foreach (var @event in events.Values)
            {
                if (!@event.TryGetTarget(out var eventTarget))
                {
                    continue;
                }
                eventTarget.ListenEvent(handler, phase);
                flag = true;
            }
            return flag;
        }

        if (key == EventKey.SomeOne)
        {
            return ListenSomeOne();
        }

        key ??= _nullKey;
        if (!events.TryGetValue(key, out var @ref)) return false;
        if (!@ref.TryGetTarget(out var target))
        {
            events.Remove(key);
            return false;
        }
        target.ListenEvent(handler, phase);
        return true;

        bool ListenSomeOne()
        {
            while (events.Count > 0)
            {
                var @event = events.First().Value;
                if (!@event.TryGetTarget(out var eventTarget))
                {
                    events.Remove(events.First().Key);
                    continue;
                }
                eventTarget.ListenEvent(handler, phase);
                return true;
            }
            return false;
        }
    }

    public bool TryListenEvent<T>(string eventName, object? key, Functor<T> handler, EventPhase phase)
    {
        if (!_events.TryGetValue(eventName, out var events)) return false;//no event

        if (key == EventKey.SomeOneButNullPrefer)
        {
            // ReSharper disable once InvertIf
            if (events.TryGetValue(_nullKey, out var nullEvent)) //try to get event with null-key
            {
                if (nullEvent.TryGetTarget(out var nullTarget) && nullTarget is IEvent<T> nullTargetT)
                {
                    nullTargetT.ListenEvent(handler, phase);
                    return true;
                }
                events.Remove(_nullKey);
            }
            return ListenSomeOne();
        }

        if (key == EventKey.All)
        {
            var flag = false;
            foreach (var @event in events.Values)
            {
                if (!@event.TryGetTarget(out var eventTarget) || eventTarget is not IEvent<T> eventTargetT)
                {
                    continue;
                }
                eventTargetT.ListenEvent(handler, phase);
                flag = true;
            }
            return flag;
        }

        if (key == EventKey.SomeOne)
        {
            return ListenSomeOne();
        }

        key ??= _nullKey;
        if (!events.TryGetValue(key, out var @ref)) return false;
        if (!@ref.TryGetTarget(out var target))
        {
            events.Remove(key);
            return false;
        }
        if (target is not IEvent<T> targetT) return false;
        targetT.ListenEvent(handler, phase);
        return true;

        bool ListenSomeOne()
        {
            while (events.Count > 0)
            {
                var @event = events.First().Value;
                if (!@event.TryGetTarget(out var eventTarget))
                {
                    events.Remove(events.First().Key);
                    continue;
                }

                if (eventTarget is not IEvent<T> eventTargetT) continue;
                eventTargetT.ListenEvent(handler, phase);
                return true;

            }
            return false;
        }
    }

    public void RemoveEvent(string eventName, object? key, EventParam param, bool doUnload)
    {
        if (!_events.TryGetValue(eventName, out var events)) return;
        if (key == EventKey.All)
        {
            if (doUnload)
            {
                foreach (var eventKv in events)
                {
                    if (eventKv.Value.TryGetTarget(out var eventTarget))
                        eventTarget.Unload(param);
                }
            }
            _events.Remove(eventName);
            return;
        }
        if (key == EventKey.SomeOne)
        {
            RemoveSomeOne();
        }
        if (key == EventKey.SomeOneButNullPrefer)
        {
            if (events.TryGetValue(_nullKey, out var nullEvent))
            {
                if (nullEvent.TryGetTarget(out var nullTarget))
                {
                    if (doUnload)
                    {
                        nullTarget.Unload(param);
                    }
                    events.Remove(_nullKey);
                }
            }
            RemoveSomeOne();
        }

        key ??= _nullKey;
        if (!events.TryGetValue(key, out var @ref)) return;
        if (@ref.TryGetTarget(out var target))
        {
            target.Unload(param);
        }
        events.Remove(key);
        return;

        void RemoveSomeOne()
        {
            while (events.Count > 0)
            {
                var @event = events.First().Value;
                if (!@event.TryGetTarget(out var eventTarget))
                {
                    events.Remove(events.First().Key);
                    continue;
                }
                if (doUnload)
                {
                    eventTarget.Unload(param);
                }
                events.Remove(events.First().Key);
                return;
            }
        }
    }

    private readonly Dictionary<string, Dictionary<object, WeakReference<IEvent>>> _events = [];
}