using StarGate.Adapter;
using StarGate.Fundamental;
using System.Collections;
using System.Threading.Tasks;

namespace StarGate.Abstractions
{
    public interface IStarBelt<T>
    {
        StarResult<T> Invoke(IStarParam param);
        void RegisterHandler(Functor<T> handler);

        #region derived functions
        StarResult<T> Invoke(IEnumerable param) => Invoke(new StarParam(param));
        StarResult<T> Invoke(IDictionary param) => Invoke(new StarParam(param));
        StarResult<T> Invoke(params object?[] param) => Invoke(new StarParam(param));

        async Task<StarResult<T>> InvokeAsync(IStarParam param) => await Task.Run(() => Invoke(param));
        Task<StarResult<T>> InvokeAsync(IEnumerable param) => InvokeAsync(new StarParam(param));
        Task<StarResult<T>> InvokeAsync(IDictionary param) => InvokeAsync(new StarParam(param));
        Task<StarResult<T>> InvokeAsync(params object?[] param) => InvokeAsync(new StarParam(param));
        #endregion
    }
}
