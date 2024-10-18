using Microsoft.Extensions.DependencyInjection;
using StarGate.Attributes;
using System;
using System.Reflection;

namespace StarGate.Extensions
{
    public static class AddStarGateExtension
    {
        public static IServiceCollection AddStarTrain(this IServiceCollection collection, Assembly assembly)
        {
            var providerTypes = assembly.GetTypes();

            foreach (var type in providerTypes)
            {
                if (type.GetCustomAttribute<StarProviderAttribute>() is { } t)
                {

                }
            }

            foreach (var provider in providerTypes)
            {
                Providers[provider.Name] = Activator.CreateInstance(provider.Type);
                RegisterObservers(provider.Name, provider.Type, assembly, collection);
            }

            // 注册 Providers 到 DI 容器
            foreach (var provider in Providers)
            {
                collection.AddSingleton(provider.Value);
            }

            return collection;
        }

        public static IServiceCollection AddStarTrain(this IServiceCollection collection, Type type) =>
            collection.AddStarTrain(type.Assembly);

        public static IServiceCollection AddStarTrain<T>(this IServiceCollection collection) =>
            collection.AddStarTrain(typeof(T).Assembly);
    }
}
