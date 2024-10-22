using System;
using System.Reflection;
using System.Threading.Tasks;

namespace StarGate.Fundamental;

internal static class MethodHelper
{
    private static async Task<object?> FromTask(Task task)
    {
        await task;
        try
        {
            return ((dynamic)task).Result;
        }
        catch (Exception)
        {
            return null;
        }
    }

    internal static object? GetResult(MethodInfo method, object? target, params object?[]? param)
    {
        var returnType = method.ReturnType;

        // Task<T>
        if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
        {
            if (method.Invoke(target, param) is not Task taskInstance)
                throw new InvalidOperationException("Method invocation returned null for Task.");

            return FromTask(taskInstance).GetAwaiter().GetResult();
        }

        // Task
        if (returnType == typeof(Task))
        {
            if (method.Invoke(target, param) is not Task taskInstance)
                throw new InvalidOperationException("Method invocation returned null for Task.");

            taskInstance.GetAwaiter().GetResult();
            return default!;
        }

        // SyncMethod
        var result = method.Invoke(target, param);
        return result;
    }

    internal static async Task<object?> GetAndStartTask(MethodInfo method, object? target, params object?[]? param)
    {
        var returnType = method.ReturnType;

        // Task<T>
        if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
        {
            if (method.Invoke(target, param) is not Task taskInstance)
                throw new InvalidOperationException("Method invocation returned null for Task.");

            return await FromTask(taskInstance);
        }

        // Task
        if (returnType == typeof(Task))
        {
            if (method.Invoke(target, param) is not Task taskInstance)
                throw new InvalidOperationException("Method invocation returned null for Task.");

            return await taskInstance.ContinueWith(_ => (object?)null);
        }

        // SyncMethod
        return await Task.Run(() => method.Invoke(target, param));
    }
}