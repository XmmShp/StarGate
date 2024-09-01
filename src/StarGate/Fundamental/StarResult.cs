using StarGate.Enums;

namespace StarGate.Fundamental
{
    public class StarResult<T>
    {
        public StarStatus Status { get; set; }
        public T? Return { get; }

        public StarResult(T? @return, StarStatus status)
        {
            Return = @return;
            Status = status;
        }

        public static implicit operator StarResult<T>((T? @return, StarStatus status) tuple) => new(tuple.@return, tuple.status);
        public static implicit operator StarResult<T>((StarStatus status, T? @return) tuple) => new(tuple.@return, tuple.status);
        public static implicit operator StarResult<T>(T? @return) => new(@return, StarStatus.Solve);
        public static implicit operator StarResult<T>(StarStatus status) => new(default, status);
        public static implicit operator T?(StarResult<T> starResult) => starResult.Return;
        public static implicit operator StarStatus(StarResult<T> result) => result.Status;
    }
}
