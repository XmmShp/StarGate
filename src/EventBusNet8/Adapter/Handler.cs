using System.Reflection;
using EventBusNet8.Abstractions;
using EventBusNet8.Enums;
using EventBusNet8.Fundamental;

namespace EventBusNet8.Adapter;
public delegate EventResult Functor(IEventParam eventParam);
public delegate EventResult<T> Functor<T>(IEventParam eventParam);
internal class Handler
{
    protected Handler() { }
    public Handler(Functor functor)
    {
        Method = functor.Method;
        Target = functor.Target is null ? null : new WeakReference(functor.Target);
    }
    public EventResult Invoke(IEventParam param) => Target is null ? (EventResult)Method.Invoke(null, [param])!
        : IsAlive ? (EventResult)Method.Invoke(Target.Target, [param])!
        : EventStatus.Continue;

    public bool IsAlive => Target?.IsAlive ?? true;
    protected MethodInfo Method = null!;
    protected WeakReference? Target;
}

internal class Handler<T> : Handler
{
    public Handler(Functor<T> functor)
    {
        Method = functor.Method;
        Target = functor.Target is null ? null : new WeakReference(functor.Target);
    }
    public new EventResult<T> Invoke(IEventParam param) => Target is null ? (EventResult<T>)Method.Invoke(null, [param])!
        : IsAlive ? (EventResult<T>)Method.Invoke(Target.Target, [param])!
        : EventStatus.Continue;
}