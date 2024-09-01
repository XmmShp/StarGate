using StarGate.Adapter;
using StarGate.Enums;
using System;

namespace StarGate.Abstractions
{
    /// <summary>
    /// Defines an interface for a star gate, responsible for managing and dispatching events.
    /// </summary>
    public interface IStarGate
    {
        bool TryAllocateStar(string starName, object? key, out IStar value);
        bool TryAllocateStar<T>(string starName, object? key, out IStar<T> value);
        bool TrySubscribeStar(string starName, object? key, Functor handler, StarPhase phase = StarPhase.On);
        bool TrySubscribeStar<T>(string starName, object? key, Functor<T> handler, StarPhase phase = StarPhase.On);
        void RemoveStar(IStar star);

        #region Derived Functions
        bool TryAllocateStar(string starName, out IStar value) => TryAllocateStar(starName, null, out value);
        IStar AllocateStar(string starName, object? key)
        {
            if (key == StarKey.SomeOneButNullPrefer || key == StarKey.SomeOne || key == StarKey.All)
                throw new ArgumentException("Special Key is not supported here.");
            // ReSharper disable once SuggestVarOrType_SimpleTypes
            return TryAllocateStar(starName, key, out IStar value) ? value : throw new InvalidOperationException($"Event \"{starName}\" already exists.");
        }
        IStar AllocateStar(string starName) => AllocateStar(starName, null);

        bool TryAllocateStar<T>(string starName, out IStar<T> value) => TryAllocateStar(starName, null, out value);
        IStar<T> AllocateStar<T>(string starName, object? key)
        {
            if (key == StarKey.SomeOneButNullPrefer || key == StarKey.SomeOne || key == StarKey.All)
                throw new ArgumentException("Special Key is not supported here.");
            return TryAllocateStar(starName, key, out IStar<T> value) ? value : throw new InvalidOperationException($"Event \"{starName}\" already exists.");
        }
        IStar<T> AllocateStar<T>(string starName) => AllocateStar<T>(starName, null!);

        bool TrySubscribeStar(string starName, Functor handler, StarPhase phase = StarPhase.On) => TrySubscribeStar(starName, StarKey.SomeOneButNullPrefer, handler, phase);
        void SubscribeStar(string starName, object? key, Functor handler, StarPhase phase = StarPhase.On)
        {
            if (!TrySubscribeStar(starName, key, handler, phase))
                throw new InvalidOperationException($"Event does not exist.");
        }
        void SubscribeStar(string starName, Functor handler, StarPhase phase = StarPhase.On) => SubscribeStar(starName, StarKey.SomeOneButNullPrefer, handler, phase);

        bool TrySubscribeStar<T>(string starName, Functor<T> handler, StarPhase phase = StarPhase.On) => TrySubscribeStar(starName, StarKey.SomeOneButNullPrefer, handler, phase);
        void SubscribeStar<T>(string starName, object? key, Functor<T> handler, StarPhase phase = StarPhase.On)
        {
            if (!TrySubscribeStar(starName, key, handler, phase))
                throw new InvalidOperationException("Event does not exist or return type is not compatible.");
        }
        void SubscribeStar<T>(string starName, Functor<T> handler, StarPhase phase = StarPhase.On) => SubscribeStar(starName, StarKey.SomeOneButNullPrefer, handler, phase);
        #endregion
    }
}
