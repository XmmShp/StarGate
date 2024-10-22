using System;
using System.Reflection;
using System.Threading.Tasks;

namespace StarGate.Fundamental;

internal static class MethodHelper
{
    internal static object? GetResult(MethodInfo method, object? target, params object?[]? param)
    {
        var returnType = method.ReturnType;

        // Task<T>
        if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
        {
            var taskType = returnType.GetGenericArguments()[0];

            if (method.Invoke(target, param) is not Task taskInstance)
                throw new InvalidOperationException("Method invocation returned null for Task.");

            if (taskType != typeof(void)) return ((Task<object>)taskInstance).GetAwaiter().GetResult();

            taskInstance.GetAwaiter().GetResult();
            return default!;
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
}