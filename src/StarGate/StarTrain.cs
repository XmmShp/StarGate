using Microsoft.Extensions.DependencyInjection;
using StarGate.Abstractions;
using StarGate.Fundamental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace StarGate
{
    public class StarTrain : IStarTrain
    {
        private readonly IServiceProvider _provider;
        private readonly Dictionary<string, List<MethodInfo>> _observers;

        public StarTrain(IServiceProvider provider,
            IDictionary<string, List<MethodInfo>> observers)
        {
            _provider = provider;
            _observers = new Dictionary<string, List<MethodInfo>>(observers);
        }


        public IList<T> Broadcast<T>(string starName, IStarParam param)
        {
            using var scope = _provider.CreateScope();
            var ret = new List<T>();
            var observers = _observers[starName];
            foreach (var observerMethod in observers)
            {
                if (observerMethod.ReflectedType is null)
                {
                    ret.Add(MethodHelper.GetResult<T>(observerMethod, null, param));
                    continue;
                }

                var service = scope.ServiceProvider.GetRequiredService(observerMethod.ReflectedType);
                ret.Add(MethodHelper.GetResult<T>(observerMethod, service, param));
            }
            return ret;
        }

        public async Task<IList<T>> BroadcastAsync<T>(string starName, IStarParam param)
        {
            using var scope = _provider.CreateScope();
            var tasks = new List<Task<T>>();
            var observers = _observers[starName];
            foreach (var observerMethod in observers)
            {
                if (observerMethod.ReflectedType is null)
                {
                    tasks.Add(MethodHelper.GetTask<T>(observerMethod, null, param));
                    continue;
                }

                var service = scope.ServiceProvider.GetRequiredService(observerMethod.ReflectedType);
                tasks.Add(MethodHelper.GetTask<T>(observerMethod, service, param));
            }

            await Task.WhenAll(tasks);

            var results = tasks.Select(task => task.Result).ToList();
            return results;
        }
    }
}
