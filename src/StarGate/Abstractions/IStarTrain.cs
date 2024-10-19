using StarGate.Fundamental;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarGate.Abstractions
{
    public interface IStarTrain
    {
        public IList<T> Broadcast<T>(string starName, IStarParam param);
        public IList<T> Broadcast<T>(string starName, params object?[]? param)
            => Broadcast<T>(starName, new StarParam(param));
        public void Broadcast(string starName, IStarParam param)
            => Broadcast<VoidType>(starName, param);
        public void Broadcast(string starName, params object?[]? param)
            => Broadcast(starName, new StarParam(param));

        public bool TryBroadcast<T>(string starName, IStarParam param, out IList<T> result)
        {
            try
            {
                result = Broadcast<T>(starName, param);
                return true;
            }
            catch
            {
                result = default!;
                return false;
            }
        }

        public Task<IList<T>> BroadcastAsync<T>(string starName, IStarParam param);
        public Task BroadcastAsync(string starName, IStarParam param) => BroadcastAsync<VoidType>(starName, param);
    }
}