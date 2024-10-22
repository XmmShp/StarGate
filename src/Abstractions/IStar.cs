using StarGate.Adapter;
using StarGate.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarGate.Abstractions
{
    public interface IStar
    {
        string Name { get; }
        object? Key { get; }
        void Unload(IStarParam param);
        Task UnloadAsync(IStarParam param);
    }

    public interface IStar<T> : IStar
    {
        IList<T> Invoke(IStarParam param);
        Task<IList<T>> InvokeAsync(IStarParam param);
        void RegisterHandler(Functor<T> handler, StarPhase phase);

        #region derived functions
        #region invoke
        IList<T> Invoke(IEnumerable param) => Invoke(new StarParam(param));
        IList<T> Invoke(IDictionary param) => Invoke(new StarParam(param));
        IList<T> Invoke(params object?[] param) => Invoke(new StarParam(param));
        #endregion
        #region invoke async
        Task<IList<T>> InvokeAsync(IEnumerable param) => InvokeAsync(new StarParam(param));
        Task<IList<T>> InvokeAsync(IDictionary param) => InvokeAsync(new StarParam(param));
        Task<IList<T>> InvokeAsync(params object?[] param) => InvokeAsync(new StarParam(param));
        #endregion
        #endregion

    }
}
