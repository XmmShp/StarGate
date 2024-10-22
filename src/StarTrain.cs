using Microsoft.Extensions.DependencyInjection;
using StarGate.Abstractions;
using StarGate.Fundamental;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace StarGate
{
    public class StarTrain : IStarTrain
    {
        private readonly IServiceProvider _provider;
        private readonly IDictionary<string, List<MethodInfo>> _observers;

        public StarTrain(IServiceProvider provider,
            IDictionary<string, List<MethodInfo>> observers)
        {
            _provider = provider;
            _observers = observers;
        }


        public IList Broadcast(string starName, IStarParam param)
        {
            using var scope = _provider.CreateScope();
            var ret = new List<object?>();
            if (!_observers.TryGetValue(starName, out var observers)) return ret;
            foreach (var observerMethod in observers)
            {
                object? result;

                if (observerMethod.ReflectedType is null)
                {
                    result = MethodHelper.GetResult(observerMethod, null, param);
                    ret.Add(result);
                    continue;
                }

                var service = scope.ServiceProvider.GetRequiredService(observerMethod.ReflectedType);
                result = MethodHelper.GetResult(observerMethod, service, param);
                ret.Add(result);
            }
            return ret;
        }

        public async Task<IList> BroadcastAsync(string starName, IStarParam param)
        {
            using var scope = _provider.CreateScope();
            var tasks = new List<Task<object?>>();

            if (!_observers.TryGetValue(starName, out var observers)) return await Task.FromResult(tasks as IList);

            foreach (var observerMethod in observers)
            {
                if (observerMethod.ReflectedType is null)
                {
                    tasks.Add(MethodHelper.GetAndStartTask(observerMethod, null, param));
                    continue;
                }

                var service = scope.ServiceProvider.GetRequiredService(observerMethod.ReflectedType);
                tasks.Add(MethodHelper.GetAndStartTask(observerMethod, service, param));
            }

            await Task.WhenAll(tasks);

            return tasks.Select(task => task.Result).ToList();
        }
    }
}
