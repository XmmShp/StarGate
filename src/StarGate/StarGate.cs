﻿using StarGate.Abstractions;
using StarGate.Adapter;
using StarGate.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StarGate
{

    public class StarGate : IStarGate
    {
        #region interfaces

        public IStar<T> AllocateStar<T>(string starName, object? key)
        {
            if (key == StarKey.All || key == StarKey.SomeOne || key == StarKey.SomeOneButNullPrefer)
            {
                throw new ArgumentException("Specific Key is not allowed here.");
            }
            key ??= StarKey.NullKey;
            if (_stars.TryGetValue(starName, out var refs)
                && refs.TryGetValue(key, out var cur)
                && cur.TryGetTarget(out _))
            {
                throw new ArgumentException($"Event {starName} was existed.");
            }
            var star = new Star<T>(starName, key, this);
            _stars.TryAdd(starName, new Dictionary<object, WeakReference<IStar>>());
            _stars[starName][key] = new WeakReference<IStar>(star);
            return star;
        }
        public void SubscribeStar<T>(string starName, object? key, Functor<T> handler, StarPhase phase = StarPhase.On)
        {
            if (!_stars.TryGetValue(starName, out var events))
            {
                throw new ArgumentException($"Event {starName} is not existed.");
            } //no event

            if (key == StarKey.SomeOneButNullPrefer)
            {
                // ReSharper disable once InvertIf
                if (events.TryGetValue(StarKey.NullKey, out var nullEvent)) //try to get event with null-key
                {
                    if (nullEvent.TryGetTarget(out var nullTarget) && nullTarget is IStar<T> nullTargetT)
                    {
                        nullTargetT.RegisterHandler(handler, phase);
                        return;
                    }
                    events.Remove(StarKey.NullKey);
                }
                ListenSomeOne();
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

                if (!flag)
                {
                    throw new ArgumentException($"Event {starName} is not existed.");
                }
            }

            if (key == StarKey.SomeOne)
            {
                ListenSomeOne();
            }

            key ??= StarKey.NullKey;
            if (!events.TryGetValue(key, out var @ref))
            {
                throw new ArgumentException($"Event {starName} with key: {key} is not existed.");
            }
            if (!@ref.TryGetTarget(out var target))
            {
                events.Remove(key);
                return;
            }

            if (target is not IStar<T> targetT)
            {
                throw new InvalidCastException($"Event {starName} with key:{key} is not of type {typeof(T).Name}");
            }
            targetT.RegisterHandler(handler, phase);
            return;

            void ListenSomeOne()
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
                    return;
                }
            }
        }

        public void RemoveStar(IStar star) => RemoveStar(star.Name, star.Key, new StarParam(), false);

        #endregion

        public void RemoveStar(string starName, StarParam param) => RemoveStar(starName, StarKey.All, param, true);
        public void RemoveStar(string starName) => RemoveStar(starName, new StarParam());
        public void RemoveStar(string starName, object? key, IStarParam param, bool doUnload)
        {
            if (!_stars.TryGetValue(starName, out var events)) return;
            if (key == StarKey.All)
            {
                if (doUnload)
                {
                    foreach (var @event in events.Values)
                    {
                        if (@event.TryGetTarget(out var eventTarget))
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
                if (events.TryGetValue(StarKey.NullKey, out var nullEvent))
                {
                    if (nullEvent.TryGetTarget(out var nullTarget))
                    {
                        if (doUnload)
                        {
                            nullTarget.Unload(param);
                        }
                        events.Remove(StarKey.NullKey);
                    }
                }
                RemoveSomeOne();
            }

            key ??= StarKey.NullKey;
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

