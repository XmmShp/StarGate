using System.Reflection;
using EventBusNet8.Abstractions;
using EventBusNet8.Enums;
using EventBusNet8.Fundamental;

namespace EventBusNet8.Adapter;
public class Handler
{
    public delegate EventResult Functor(IEventParam eventParam);
    public Handler()
    {
        Method = null!;
        Target = null!;
    }
    public Handler(Functor functor)
    {
        Method = functor.Method;
        Target = functor.Target is null ? null : new WeakReference(functor.Target);
    }
    public EventResult Invoke(IEventParam param)
    {
        if (Target is null)
        {
            return (EventResult)Method.Invoke(null, [param])!;
        }
        if (IsAlive)
        {
            return (EventResult)Method.Invoke(Target.Target, [param])!;
        }
        return EventStatus.Continued;
    }

    public static implicit operator Handler(Functor functor) => new(functor);

    public bool IsAlive => Target?.IsAlive ?? true;
    protected MethodInfo Method;
    protected WeakReference? Target;
}

public class Handler<T> : Handler
{
    public new delegate EventResult<T> Functor(IEventParam eventParam);
    public Handler(Functor functor)
    {
        Method = functor.Method;
        Target = functor.Target is null ? null : new WeakReference(functor.Target);
    }
    public new EventResult<T> Invoke(IEventParam param)
    {
        if (Target is null)
        {
            return (EventResult<T>)Method.Invoke(null, [param])!;
        }
        if (IsAlive)
        {
            return (EventResult<T>)Method.Invoke(Target.Target, [param])!;
        }

        return EventStatus.Continued;
    }
    
    public static implicit operator Handler<T>(Functor functor) => new(functor);
}