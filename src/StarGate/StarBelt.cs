using StarGate.Abstractions;
using StarGate.Adapter;
using StarGate.Enums;
using System.Collections.Generic;
using System.Linq;

namespace StarGate
{
    public class StarBelt<T> : IStarBelt<T>
    {
        public StarBelt() { _handlers.Add(new NullStarHandler<T>()); }
        public T Invoke(IStarParam param)
        {
            T result = default!;
            foreach (var handler in _handlers.TakeWhile(_ => !param.Status.IsInterrupted()))
            {
                var res = handler.Invoke(param);
                if (!param.Status.IsSolved()) continue;
                result = res;
                break;
            }
            return result;
        }

        public void RegisterHandler(Functor<T> handler)
        {
            _handlers.Insert(_handlers.Count - 1, new StarHandler<T>(handler));
        }

        private readonly List<StarHandler<T>> _handlers = new();
    }
}
