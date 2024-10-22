using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarGate.Abstractions
{
    public interface IStarTrain
    {
        protected IList Broadcast(Type responseType, string starName, IStarParam param);

        public IList<TResponse> Broadcast<TResponse>(string starName, IStarParam param)
            => Broadcast(typeof(TResponse), starName, param).OfType<TResponse>().ToList();

        public IList<TResponse> Broadcast<TResponse>(string starName, params object?[]? param)
            => Broadcast<TResponse>(starName, new StarParam(param));

        public void Broadcast(string starName, IStarParam param)
            => Broadcast(typeof(void), starName, param);

        public void Broadcast(string starName, params object?[]? param)
            => Broadcast(starName, new StarParam(param));

        protected Task<IList> BroadcastAsync(Type responseType, string starName, IStarParam param);

        public async Task<IList<TResponse>> BroadcastAsync<TResponse>(string starName, IStarParam param)
            => (await BroadcastAsync(typeof(TResponse), starName, param)).OfType<TResponse>().ToList();

        public async Task BroadcastAsync(string starName, IStarParam param)
            => await BroadcastAsync(typeof(void), starName, param);
    }
}