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
        IStar<T> AllocateStar<T>(string starName, object? key);
        void SubscribeStar<T>(string starName, object? key, Functor<T> handler, StarPhase phase = StarPhase.On);
        void RemoveStar(IStar star);
        #endregion

        #region derived functions

        #region Allocate Star
        IStar<T> AllocateStar<T>(string starName) => AllocateStar<T>(starName, null);
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
        #endregion

        #region SubscribeStar
        void SubscribeStar<T>(string starName, Functor<T> handler, StarPhase phase = StarPhase.On) => SubscribeStar(starName, StarKey.SomeOneButNullPrefer, handler, phase);
        bool TrySubscribeStar<T>(string starName, Functor<T> handler, StarPhase phase = StarPhase.On) => TrySubscribeStar(starName, StarKey.SomeOneButNullPrefer, handler, phase);
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
