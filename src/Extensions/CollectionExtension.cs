using Microsoft.Extensions.DependencyInjection;
using StarGate.Abstractions;
using StarGate.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StarGate.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddStarTrain(this IServiceCollection collection, Assembly assembly)
        {
            var observerInfos = new Dictionary<string, List<MethodInfo>>();
            var observerMethods = assembly.GetTypes()
                .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                    .Where(m => m.GetCustomAttributes<StarObserverAttribute>(true).Any()));

            foreach (var method in observerMethods)
            {
                var attributes = method.GetCustomAttributes<StarObserverAttribute>(true);
                foreach (var attribute in attributes)
                {
                    observerInfos.TryAdd(attribute.Name, new List<MethodInfo>());
                    observerInfos[attribute.Name].Add(method);
                    if (method.ReflectedType is { } type)
                        collection.Add(new ServiceDescriptor(type, type, attribute.ScopeType));
                }
            }

            collection.AddSingleton<IStarTrain>(provider => new StarTrain(provider, observerInfos));
            return collection;
        }

        public static IServiceCollection AddStarTrain(this IServiceCollection collection) =>
            collection.AddStarTrain(Assembly.GetCallingAssembly());

        public static IServiceCollection AddStarTrain(this IServiceCollection collection, Type type) =>
            collection.AddStarTrain(type.Assembly);

        public static IServiceCollection AddStarTrain<T>(this IServiceCollection collection) =>
            collection.AddStarTrain(typeof(T).Assembly);
    }
}
