using StarGate;
using StarGate.Abstractions;
using StarGate.Adapter;
using StarGate.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventBusNet8
{
    internal class NullKey { }


    public class StarGate : IStarGate
    {
        private readonly object _nullKey = new NullKey();

        public void RemoveStar(IStar star) => RemoveStar(star.Name, star.Key, new StarParam(), false);
        public void RemoveStar(string starName, StarParam param) => RemoveStar(starName, StarKey.All, param, true);
        public void RemoveStar(string starName) => RemoveStar(starName, new StarParam());

        public bool TryAllocateStar(string starName, object? key, out IStar value)
        {
            if (key == StarKey.All || key == StarKey.SomeOne || key == StarKey.SomeOneButNullPrefer)
            {
                value = default!;
                return false;
            }
            key ??= _nullKey;
            if (_stars.TryGetValue(starName, out var refs) && refs.TryGetValue(key, out var cur) && cur.TryGetTarget(out _))
            {
                value = default!;
                return false;
            }
            value = new Star(starName, key, this);
            _stars.TryAdd(starName, new Dictionary<object, WeakReference<IStar>>());
            _stars[starName][key] = new WeakReference<IStar>(value);
            return true;
        }

        public bool TryAllocateStar<T>(string starName, object? key, out IStar<T> value)
        {
            if (key == StarKey.All || key == StarKey.SomeOne || key == StarKey.SomeOneButNullPrefer)
            {
                value = default!;
                return false;
            }
            key ??= _nullKey;
            if (_stars.TryGetValue(starName, out var refs) && refs.TryGetValue(key, out var cur) && cur.TryGetTarget(out _))
            {
                value = default!;
                return false;
            }
            value = new Star<T>(starName, key, this);
            _stars.TryAdd(starName, new Dictionary<object, WeakReference<IStar>>());
            _stars[starName][key] = new WeakReference<IStar>(value);
            return true;
        }

        public bool TrySubscribeStar(string starName, object? key, Functor handler, StarPhase phase)
        {
            if (!_stars.TryGetValue(starName, out var events)) return false;//no event

            if (key == StarKey.SomeOneButNullPrefer)
            {
                // ReSharper disable once InvertIf
                if (events.TryGetValue(_nullKey, out var nullEvent))//try to get event with null-key
                {
                    if (nullEvent.TryGetTarget(out var nullTarget))
                    {
                        nullTarget.RegisterHandler(handler, phase);
                        return true;
                    }
                    events.Remove(_nullKey);
                }
                return ListenSomeOne();
            }

            if (key == StarKey.All)
            {
                var flag = false;
                foreach (var @event in events.Values)
                {
                    if (!@event.TryGetTarget(out var eventTarget))
                    {
                        continue;
                    }
                    eventTarget.RegisterHandler(handler, phase);
                    flag = true;
                }
                return flag;
            }

            if (key == StarKey.SomeOne)
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
            target.RegisterHandler(handler, phase);
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
                    eventTarget.RegisterHandler(handler, phase);
                    return true;
                }
                return false;
            }
        }

        public bool TrySubscribeStar<T>(string starName, object? key, Functor<T> handler, StarPhase phase)
        {
            if (!_stars.TryGetValue(starName, out var events)) return false;//no event

            if (key == StarKey.SomeOneButNullPrefer)
            {
                // ReSharper disable once InvertIf
                if (events.TryGetValue(_nullKey, out var nullEvent)) //try to get event with null-key
                {
                    if (nullEvent.TryGetTarget(out var nullTarget) && nullTarget is IStar<T> nullTargetT)
                    {
                        nullTargetT.RegisterHandler(handler, phase);
                        return true;
                    }
                    events.Remove(_nullKey);
                }
                return ListenSomeOne();
            }

            if (key == StarKey.All)
            {
                var flag = false;
                foreach (var @event in events.Values)
                {
                    if (!@event.TryGetTarget(out var eventTarget) || eventTarget is not IStar<T> eventTargetT)
                    {
                        continue;
                    }
                    eventTargetT.RegisterHandler(handler, phase);
                    flag = true;
                }
                return flag;
            }

            if (key == StarKey.SomeOne)
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
            if (target is not IStar<T> targetT) return false;
            targetT.RegisterHandler(handler, phase);
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

                    if (eventTarget is not IStar<T> eventTargetT) continue;
                    eventTargetT.RegisterHandler(handler, phase);
                    return true;

                }
                return false;
            }
        }

        public void RemoveStar(string starName, object? key, StarParam param, bool doUnload)
        {
            if (!_stars.TryGetValue(starName, out var events)) return;
            if (key == StarKey.All)
            {
                if (doUnload)
                {
                    foreach (var eventKv in events)
                    {
                        if (eventKv.Value.TryGetTarget(out var eventTarget))
                            eventTarget.Unload(param);
                    }
                }
                _stars.Remove(starName);
                return;
            }
            if (key == StarKey.SomeOne)
            {
                RemoveSomeOne();
            }
            if (key == StarKey.SomeOneButNullPrefer)
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

        private readonly Dictionary<string, Dictionary<object, WeakReference<IStar>>> _stars = new();
    }
}

