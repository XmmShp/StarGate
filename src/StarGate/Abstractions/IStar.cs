using StarGate.Adapter;
using StarGate.Enums;
using StarGate.Fundamental;
using System.Collections;
using System.Threading.Tasks;

namespace StarGate.Abstractions
{
    public interface IStar
    {
        string Name { get; }
        object Key { get; }
        StarResult Invoke(StarParam param);
        Task<StarResult> InvokeAsync(StarParam param);
        void RegisterHandler(Functor handler, StarPhase phase);
        void Unload(StarParam param);

        #region Derived Functions
        StarResult Invoke(IEnumerable param) => Invoke(new StarParam(param));
        StarResult Invoke(IDictionary param) => Invoke(new StarParam(param));
        StarResult Invoke(params object?[] param) => Invoke(new StarParam(param));
        Task<StarResult> InvokeAsync(IEnumerable param) => InvokeAsync(new StarParam(param));
        Task<StarResult> InvokeAsync(IDictionary param) => InvokeAsync(new StarParam(param));
        Task<StarResult> InvokeAsync(params object?[] param) => InvokeAsync(new StarParam(param));
        #endregion
    }

    public interface IStar<T> : IStar
    {
        new StarResult<T> Invoke(StarParam param);
        new Task<StarResult<T>> InvokeAsync(StarParam param);
        void RegisterHandler(Functor<T> handler, StarPhase phase);

        #region Derived Functions
        new StarResult<T> Invoke(IEnumerable param) => Invoke(new StarParam(param));
        new StarResult<T> Invoke(IDictionary param) => Invoke(new StarParam(param));
        new StarResult<T> Invoke(params object?[] param) => Invoke(new StarParam(param));
        new Task<StarResult<T>> InvokeAsync(IEnumerable param) => InvokeAsync(new StarParam(param));
        new Task<StarResult<T>> InvokeAsync(IDictionary param) => InvokeAsync(new StarParam(param));
        new Task<StarResult<T>> InvokeAsync(params object?[] param) => InvokeAsync(new StarParam(param));
        #endregion

    }
}
