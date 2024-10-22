using StarGate.Abstractions;
using StarGate.Enums;
using System;
using System.Reflection;

namespace StarGate.Adapter
{
    public delegate T Functor<out T>(IStarParam starParam);

    internal class StarHandler<T>
    {
        internal StarHandler(Functor<T> functor)
        {
            _method = functor.Method;
            _target = functor.Target is null ? null : new WeakReference(functor.Target);
        }
        internal T Invoke(IStarParam param) => _target is null ? (T)_method.Invoke(null, new object?[] { param })!
            : IsAlive ? (T)_method.Invoke(_target.Target, new object?[] { param })!
            : default!;

        internal bool IsAlive => _target?.IsAlive ?? true;
        private readonly MethodInfo _method;
        private readonly WeakReference? _target;
    }

    internal class NullStarHandler<T> : StarHandler<T>
    {
        internal NullStarHandler() : base(
            param =>
            {
                param.Status |= StarStatus.Solved;
                return default!;
            })
        { }
    }
}
