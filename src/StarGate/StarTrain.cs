using Microsoft.Extensions.DependencyInjection;
using StarGate.Abstractions;
using StarGate.Fundamental;
using System;
using System.Collections;
using System.Collections.Generic;
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


        public IList Broadcast(Type responseType, string starName, IStarParam param)
        {
            using var scope = _provider.CreateScope();
            var ret = new List<object>();
            if (!_observers.TryGetValue(starName, out var observers)) return default!;
            foreach (var observerMethod in observers)
            {
                object? result;

                if (observerMethod.ReflectedType is null)
                {
                    result = MethodHelper.GetResult(observerMethod, null, param);
                    if (responseType.IsInstanceOfType(result))
                        ret.Add(result);
                    continue;
                }

                var service = scope.ServiceProvider.GetRequiredService(observerMethod.ReflectedType);
                result = MethodHelper.GetResult(observerMethod, service, param);
                if (responseType.IsInstanceOfType(result))
                    ret.Add(result);
            }
            return ret;
        }

        public Task<IList> BroadcastAsync(Type responseType, string starName, IStarParam param)
        {
            throw new NotImplementedException();
        }
    }
}
