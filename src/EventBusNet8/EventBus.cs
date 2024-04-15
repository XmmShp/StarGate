using EventBusNet8.Abstractions;
using EventBusNet8.Enums;

namespace EventBusNet8;

internal class NullKey;
public class EventBus : IEventBus
{
    private readonly object _nullKey = new NullKey();

    public bool TryAddEvent(string eventName, object? key, out IEvent value)
    {
        if (key == IEventBus.All || key == IEventBus.SomeOne || key == IEventBus.SomeOneButNullPrefer)
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
        if (key == IEventBus.All || key == IEventBus.SomeOne || key == IEventBus.SomeOneButNullPrefer)
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

    public bool TryListenEvent(string eventName, object? key, IHandler handler, EventPhase phase)
    {
        if (!_events.TryGetValue(eventName, out var events)) return false;//no event

        if (key == IEventBus.SomeOneButNullPrefer)
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

        if (key == IEventBus.All)
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

        if (key == IEventBus.SomeOne)
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

    public bool TryListenEvent<T>(string eventName, object? key, IHandler<T> handler, EventPhase phase)
    {
        if (!_events.TryGetValue(eventName, out var events)) return false;//no event

        if (key == IEventBus.SomeOneButNullPrefer)
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

        if (key == IEventBus.All)
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

        if (key == IEventBus.SomeOne)
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

    public void RemoveEvent(string eventName, object? key, bool doUnload)
    {
        if (!_events.TryGetValue(eventName, out var events)) return;
        if (key == IEventBus.All)
        {
            if (doUnload)
            {
                foreach (var eventKv in events)
                {
                    if (eventKv.Value.TryGetTarget(out var eventTarget))
                        eventTarget.Unload(new EventParam());
                }
            }
            _events.Remove(eventName);
            return;
        }
        if (key == IEventBus.SomeOne)
        {
            RemoveSomeOne();
        }
        if (key == IEventBus.SomeOneButNullPrefer)
        {
            if (events.TryGetValue(_nullKey, out var nullEvent))
            {
                if (nullEvent.TryGetTarget(out var nullTarget))
                {
                    if (doUnload)
                    {
                        nullTarget.Unload(new EventParam());
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
            target.Unload(new EventParam());
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
                    eventTarget.Unload(new EventParam());
                }
                events.Remove(events.First().Key);
                return;
            }
        }
    }

    private readonly Dictionary<string, Dictionary<object, WeakReference<IEvent>>> _events = [];
}