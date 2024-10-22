using System;

namespace StarGate.Enums
{
    [Flags]
    public enum StarStatus
    {
        Continue = 1,
        ParamChanged = 2,
        Interrupted = 4,
        Solved = 8,
    }

    public static class StarStatusChecker
    {
        public static bool IsContinue(this StarStatus status) => status.HasFlag(StarStatus.Continue);
        public static bool IsParamChanged(this StarStatus status) => status.HasFlag(StarStatus.ParamChanged);
        public static bool IsInterrupted(this StarStatus status) => status.HasFlag(StarStatus.Interrupted);
        public static bool IsSolved(this StarStatus status) => status.HasFlag(StarStatus.Solved);
    }
}

