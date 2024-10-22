using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarGate.Abstractions
{
    public interface IStarTrain
    {
        public IList Broadcast(string starName, IStarParam param);

        public IList Broadcast(string starName, params object?[]? param)
            => Broadcast(starName, new StarParam(param));

        public IList<TResponse> Broadcast<TResponse>(string starName, IStarParam param)
            => Broadcast(starName, param).OfType<TResponse>().ToList();

        public IList<TResponse> Broadcast<TResponse>(string starName, params object?[]? param)
            => Broadcast<TResponse>(starName, new StarParam(param));

        public Task<IList> BroadcastAsync(string starName, IStarParam param);

        public async Task<IList> BroadcastAsync(string starName, params object?[]? param)
            => await BroadcastAsync(starName, new StarParam(param));

        public async Task<IList<TResponse>> BroadcastAsync<TResponse>(string starName, IStarParam param)
            => (await BroadcastAsync(starName, param)).OfType<TResponse>().ToList();

        public async Task<IList<TResponse>> BroadcastAsync<TResponse>(string starName, params object?[]? param)
            => await BroadcastAsync<TResponse>(starName, new StarParam(param));
    }
}