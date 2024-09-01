using System;

namespace StarGate.Enums
{
    [Flags]
    public enum StarStatus
    {
        Continue = 1,//Available for StarPhase.Pre, StarPhase.On and StarPhase.Post
        ParamChange = 2,// Available for StarPhase.Pre and StarPhase.On
        Interrupt = 4,// Available for StarPhase.Pre and StarPhase.On
        Solve = 8,// Available only for StarPhase.On
        Final = Interrupt | Solve
    }

    public static class StarStatusChecker
    {
        public static bool IsContinue(this StarStatus status) => status.HasFlag(StarStatus.Continue);
        public static bool IsParamChange(this StarStatus status) => status.HasFlag(StarStatus.ParamChange);
        public static bool IsInterrupt(this StarStatus status) => status.HasFlag(StarStatus.Interrupt);
        public static bool IsSolve(this StarStatus status) => status.HasFlag(StarStatus.Solve);
    }
}

