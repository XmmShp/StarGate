using Microsoft.Extensions.DependencyInjection;
using System;

namespace StarGate.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class StarObserverAttribute : Attribute
    {
        public ServiceLifetime ScopeType { get; }
        public string Name { get; }

        public StarObserverAttribute(ServiceLifetime scopeType, string name)
        {
            ScopeType = scopeType;
            Name = name;
        }
    }
}
