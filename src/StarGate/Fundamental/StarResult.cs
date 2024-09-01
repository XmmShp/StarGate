using StarGate.Enums;

namespace StarGate.Fundamental
{
    public class StarResult
    {
        public StarResult(StarStatus status) => Status = status;
        public StarStatus Status { get; }
        public static implicit operator StarResult(StarStatus status) => new(status);
        public static implicit operator StarStatus(StarResult result) => result.Status;
    }

    public class StarResult<T> : StarResult
    {
        public StarResult(T? @return, StarStatus status) : base(status) => Return = @return;
        public T? Return { get; }
        public static implicit operator StarResult<T>((T? @return, StarStatus status) tuple) => new(tuple.@return, tuple.status);
        public static implicit operator StarResult<T>((StarStatus status, T? @return) tuple) => new(tuple.@return, tuple.status);
        public static implicit operator StarResult<T>(T? @return) => new(@return, StarStatus.Solve);
        public static implicit operator StarResult<T>(StarStatus status) => new(default, status);
        public static implicit operator T?(StarResult<T> starResult) => starResult.Return;
        public static implicit operator StarStatus(StarResult<T> result) => result.Status;
    }
}
