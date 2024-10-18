using System.Reflection;
using System.Threading.Tasks;

namespace StarGate.Fundamental;

internal static class MethodHelper
{
    internal static T GetResult<T>(MethodInfo method, object? target, params object?[]? param)
    {
        var returnType = method.ReturnType;
        if (returnType.IsAssignableFrom(typeof(Task<T>)))
        {
            return ((Task<T>)method.Invoke(target, param)!).GetAwaiter().GetResult();
        }

        if (returnType.IsAssignableFrom(typeof(Task)))
        {
            ((Task)method.Invoke(target, param)!).GetAwaiter().GetResult();
            return default!;
        }

        var result = method.Invoke(target, param);
        return (T)result!;
    }

    internal static Task<T> GetTask<T>(MethodInfo method, object? target, params object?[]? param)
    {
        var returnType = method.ReturnType;
        if (returnType.IsAssignableFrom(typeof(Task<T>)))
        {
            return (Task<T>)method.Invoke(target, param)!;
        }

        if (returnType.IsAssignableFrom(typeof(Task)))
        {
            return FromTask((Task)method.Invoke(target, param)!);
        }

        var result = Task.Run(() => (T)method.Invoke(target, param)!);
        return result;

        Task<T> FromTask(Task task) => task.ContinueWith(_ => default(T))!;
    }
}