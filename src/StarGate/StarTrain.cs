using Microsoft.Extensions.DependencyInjection;
using StarGate.Abstractions;
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
                    observerMethod.Invoke(null, new object?[] { param });
                    continue;
                }

                var service = scope.ServiceProvider.GetRequiredService(observerMethod.ReflectedType);
                if (observerMethod.Invoke(service, new object?[] { param }) is T result)
                {
                    ret.Add(result);
                }
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
                    tasks.Add(Task.Run(() => (T)observerMethod.Invoke(null, new object?[] { param })!));
                    continue;
                }

                var service = scope.ServiceProvider.GetRequiredService(observerMethod.ReflectedType);
                var task = Task.Run(async () =>
                {
                    try
                    {
                        var returnType = observerMethod.ReturnType;
                        if (returnType.IsAssignableFrom(typeof(Task<T>)))
                        {
                            return ((Task<T>)observerMethod.Invoke(service, new object?[] { param })!).GetAwaiter().GetResult();
                        }

                        if (returnType.IsAssignableFrom(typeof(Task)))
                        {
                            await (Task)observerMethod.Invoke(service, new object?[] { param })!;
                            return default;
                        }

                        var result = observerMethod.Invoke(service, new object?[] { param });
                        return (T)result!;
                    }
                    catch (TargetInvocationException ex)
                    {
                        throw ex.InnerException ?? ex;
                    }
                });

                tasks.Add(task!);
            }

            await Task.WhenAll(tasks);

            // 收集结果
            var results = tasks.Select(task => task.Result).ToList();
            return results;
        }
    }
}
