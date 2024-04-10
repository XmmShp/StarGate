using System.Reflection;
using EventBus.Abstractions;
using EventBus.Abstractions.Enums;
using EventBus.Abstractions.Fundamental;
using EventBus.Fundamental;
using static EventBus.Abstractions.IHandler;

namespace EventBus;

public class Handler : IHandler
{
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
    public IResult Invoke(IEventParam param)
    {
        if (Target is null)
        {
            return (IResult)Method.Invoke(null, [param])!;
        }
        if (IsAlive)
        {
            return (IResult)Method.Invoke(Target.Target, [param])!;
        }
        return new Result(EventStatus.Continued);
    }

    public bool IsAlive => Target?.IsAlive ?? true;
    protected MethodInfo Method;
    protected WeakReference? Target;
}

public class Handler<T> : Handler, IHandler<T>
{
    public Handler(IHandler<T>.Functor functor)
    {
        Method = functor.Method;
        Target = functor.Target is null ? null : new WeakReference(functor.Target);
    }
    public new IResult<T> Invoke(IEventParam param)
    {
        if (Target is null)
        {
            return (IResult<T>)Method.Invoke(null, [param])!;
        }
        if (IsAlive)
        {
            return (IResult<T>)Method.Invoke(Target.Target, [param])!;
        }
        return new Result<T>(default, EventStatus.Continued);
    }
}