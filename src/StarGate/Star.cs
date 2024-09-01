using StarGate.Abstractions;
using StarGate.Adapter;
using StarGate.Enums;
using StarGate.Fundamental;
using System.Collections.Generic;
using System.Linq;

namespace StarGate
{
    internal class Star<T> : IStar<T>
    {
        internal Star(string name, object key, IStarGate starGate)
        {
            Name = name;
            Key = key;
            _starGate = starGate;
        }
        private StarResult<T> InvokePhase(StarPhase phase, IStarParam param)
        {
            foreach (var handler in _handlers[phase].TakeWhile(_ => !param.Status.IsInterrupt()))
            {
                param.Status |= handler.Invoke(param).Status;
            }
            return param.Status;
        }
        public void Unload(IStarParam param)
        {
            foreach (var handler in _handlers[StarPhase.Unload])
            {
                handler.Invoke(param);
            }
            _starGate.RemoveStar(this);
        }
        public StarResult<T> Invoke(IStarParam param)
        {
            T? result = default;
            InvokePhase(StarPhase.Pre, param);
            if (!param.Status.IsInterrupt())
            {
                param.Status |= InvokePhase(StarPhase.On, param);
                foreach (var handler in _handlers[StarPhase.On].TakeWhile(_ => !param.Status.IsInterrupt()))
                {
                    var res = handler.Invoke(param);
                    param.Status |= res;
                    if (param.Status.IsSolve())
                    {
                        result = res.Return;
                    }
                }
            }
            if (!param.Status.IsInterrupt())
                InvokePhase(StarPhase.Post, param);
            return (result, param.Status);
        }

        public void RegisterHandler(Functor<T> handler, StarPhase phase)
        {
            _handlers[phase].Add(new StarHandler<T>(handler));
        }

        private readonly Dictionary<StarPhase, List<StarHandler<T>>> _handlers = new()
        {
            { StarPhase.On ,new List<StarHandler<T>>()},
        };

        public string Name { get; }
        public object? Key { get; }
        private readonly IStarGate _starGate;
    }
}

