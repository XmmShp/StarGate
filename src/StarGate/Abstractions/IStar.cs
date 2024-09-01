﻿using StarGate.Adapter;
using StarGate.Enums;
using StarGate.Fundamental;
using System.Collections;
using System.Threading.Tasks;

namespace StarGate.Abstractions
{
    public interface IStar
    {
        string Name { get; }
        object? Key { get; }
        void Unload(IStarParam param);
    }

    public interface IStar<T> : IStar
    {
        StarResult<T> Invoke(IStarParam param);
        void RegisterHandler(Functor<T> handler, StarPhase phase);

        #region Derived Functions
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
