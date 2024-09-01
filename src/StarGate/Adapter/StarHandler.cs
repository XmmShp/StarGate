using StarGate.Abstractions;
using StarGate.Enums;
using StarGate.Fundamental;
using System;
using System.Reflection;

namespace StarGate.Adapter
{
    public delegate StarResult<T> Functor<T>(IStarParam starParam);

    internal class StarHandler<T>
    {
        internal StarHandler(Functor<T> functor)
        {
            _method = functor.Method;
            _target = functor.Target is null ? null : new WeakReference(functor.Target);
        }
        internal StarResult<T> Invoke(IStarParam param) => _target is null ? (StarResult<T>)_method.Invoke(null, new object?[] { param })!
            : IsAlive ? (StarResult<T>)_method.Invoke(_target.Target, new object?[] { param })!
            : StarStatus.Continue;

        internal bool IsAlive => _target?.IsAlive ?? true;
        private readonly MethodInfo _method;
        private readonly WeakReference? _target;
    }

    internal class NullStarHandler<T> : StarHandler<T>
    {
        internal NullStarHandler() : base(param => StarStatus.Continue) { }
    }
}
