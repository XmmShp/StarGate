using System;

namespace StarGate.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class StarProviderAttribute : Attribute
    {
        public string Name { get; }

        public StarProviderAttribute(string name)
        {
            Name = name;
        }
    }
}
