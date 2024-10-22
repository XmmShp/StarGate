using StarGate.Adapter;
using System.Collections;

namespace StarGate.Abstractions
{
    public interface IStarBelt<T>
    {
        T Invoke(IStarParam param);
        void RegisterHandler(Functor<T> handler);

        #region derived functions
        T Invoke(IEnumerable param) => Invoke(new StarParam(param));
        T Invoke(IDictionary param) => Invoke(new StarParam(param));
        T Invoke(params object?[] param) => Invoke(new StarParam(param));
        #endregion
    }
}
