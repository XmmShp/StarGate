using StarGate.Abstractions;
using StarGate.Enums;
using StarGate.Fundamental;
using System;
using System.Reflection;

namespace StarGate.Adapter
{
    public delegate StarResult Functor(IStarParam starParam);
    public delegate StarResult<T> Functor<T>(IStarParam starParam);
    internal class StarHandler
    {
        protected StarHandler() { }
        public StarHandler(Functor functor)
        {
            Method = functor.Method;
            Target = functor.Target is null ? null : new WeakReference(functor.Target);
        }
        public StarResult Invoke(IStarParam param) => Target is null ? (StarResult)Method.Invoke(null, new object?[] { param })!
            : IsAlive ? (StarResult)Method.Invoke(Target.Target, new object?[] { param })!
            : StarStatus.Continue;

        public bool IsAlive => Target?.IsAlive ?? true;
        protected MethodInfo Method = null!;
        protected WeakReference? Target;
    }

    internal class StarHandler<T> : StarHandler
    {
        public StarHandler(Functor<T> functor)
        {
            Method = functor.Method;
            Target = functor.Target is null ? null : new WeakReference(functor.Target);
        }
        public new StarResult<T> Invoke(IStarParam param) => Target is null ? (StarResult<T>)Method.Invoke(null, new object?[] { param })!
            : IsAlive ? (StarResult<T>)Method.Invoke(Target.Target, new object?[] { param })!
            : StarStatus.Continue;
    }
}
