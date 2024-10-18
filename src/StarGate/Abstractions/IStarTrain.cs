using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarGate.Abstractions
{
    public interface IStarTrain
    {
        public IList<T> Broadcast<T>(string starName, IStarParam param);


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
    }
}