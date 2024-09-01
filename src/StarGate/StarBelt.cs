using StarGate.Abstractions;
using StarGate.Adapter;
using StarGate.Enums;
using StarGate.Fundamental;
using System.Collections.Generic;
using System.Linq;

namespace StarGate
{
    public class StarBelt<T> : IStarBelt<T>
    {
        public StarBelt() { _handlers.Add(new NullStarHandler<T>()); }
        public StarResult<T> Invoke(IStarParam param)
        {
            T? result = default;
            if (!param.Status.IsInterrupt())
            {
                foreach (var handler in _handlers.TakeWhile(_ => !param.Status.IsInterrupt()))
                {
                    var res = handler.Invoke(param);
                    param.Status |= res;
                    if (param.Status.IsSolve())
                    {
                        result = res.Return;
                    }
                }
            }
            return (result, param.Status);
        }

        public void RegisterHandler(Functor<T> handler)
        {
            _handlers.Add(new StarHandler<T>(handler));
        }

        private readonly List<StarHandler<T>> _handlers = new();
    }
}
