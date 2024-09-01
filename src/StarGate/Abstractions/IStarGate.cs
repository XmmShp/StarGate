using StarGate.Adapter;
using StarGate.Enums;

namespace StarGate.Abstractions
{
    /// <summary>
    /// Defines an interface for a star gate, responsible for managing and dispatching events.
    /// </summary>
    public interface IStarGate
    {
        #region interfaces
        IStar AllocateStar(string starName, object? key);
        IStar<T> AllocateStar<T>(string starName, object? key);
        void SubscribeStar(string starName, object? key, Functor handler, StarPhase phase = StarPhase.On);
        void SubscribeStar<T>(string starName, object? key, Functor<T> handler, StarPhase phase = StarPhase.On);
        void RemoveStar(IStar star);
        #endregion

        #region derived functions

        #region Allocate Star

        IStar AllocateStar(string starName) => AllocateStar(starName, null);
        IStar<T> AllocateStar<T>(string starName) => AllocateStar<T>(starName, null);
        bool TryAllocateStar(string starName, out IStar value) => TryAllocateStar(starName, null, out value);
        bool TryAllocateStar<T>(string starName, out IStar<T> value) => TryAllocateStar(starName, null, out value);

        bool TryAllocateStar<T>(string starName, object? key, out IStar<T> value)
        {
            try
            {
                value = AllocateStar<T>(starName, key);
                return true;
            }
            catch
            {
                value = default!;
                return false;
            }
        }

        bool TryAllocateStar(string starName, object? key, out IStar value)
        {
            try
            {
                value = AllocateStar(starName, key);
                return true;
            }
            catch
            {
                value = default!;
                return false;
            }
        }
        #endregion

        #region SubscribeStar

        void SubscribeStar(string starName, Functor handler, StarPhase phase = StarPhase.On) => SubscribeStar(starName, StarKey.SomeOneButNullPrefer, handler, phase);
        void SubscribeStar<T>(string starName, Functor<T> handler, StarPhase phase = StarPhase.On) => SubscribeStar(starName, StarKey.SomeOneButNullPrefer, handler, phase);

        bool TrySubscribeStar(string starName, Functor handler, StarPhase phase = StarPhase.On) => TrySubscribeStar(starName, StarKey.SomeOneButNullPrefer, handler, phase);
        bool TrySubscribeStar<T>(string starName, Functor<T> handler, StarPhase phase = StarPhase.On) => TrySubscribeStar(starName, StarKey.SomeOneButNullPrefer, handler, phase);

        bool TrySubscribeStar(string starName, object? key, Functor handler, StarPhase phase = StarPhase.On)
        {
            try
            {
                SubscribeStar(starName, key, handler, phase);
                return true;
            }
            catch
            {
                return false;
            }
        }

        bool TrySubscribeStar<T>(string starName, object? key, Functor<T> handler, StarPhase phase = StarPhase.On)
        {
            try
            {
                SubscribeStar(starName, key, handler, phase);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #endregion
    }
}
