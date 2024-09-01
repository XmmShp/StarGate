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
        Task Unload(IStarParam param);
    }

    public interface IStar<T> : IStar
    {
        Task<IList<T>> Invoke(IStarParam param);
        void RegisterHandler(Functor<T> handler, StarPhase phase);

        #region Derived Functions
        Task<IList<T>> Invoke(IEnumerable param) => Invoke(new StarParam(param));
        Task<IList<T>> Invoke(IDictionary param) => Invoke(new StarParam(param));
        Task<IList<T>> Invoke(params object?[] param) => Invoke(new StarParam(param));
        #endregion

    }
}
