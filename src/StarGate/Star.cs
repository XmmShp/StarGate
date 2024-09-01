using StarGate.Abstractions;
using StarGate.Adapter;
using StarGate.Enums;
using StarGate.Fundamental;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarGate
{
    internal class Star : IStar
    {
        public Star(string name, object key, IStarGate starGate)
        {
            Name = name;
            Key = key;
            StarGate = starGate;
        }
        public StarResult Invoke(IStarParam param)
        {
            InvokePhase(StarPhase.Pre, param);
            if (!param.Status.IsInterrupt())
                param.Status |= InvokePhase(StarPhase.On, param);
            if (!param.Status.IsInterrupt())
                InvokePhase(StarPhase.Post, param);
            return param.Status;
        }

        public void Unload(IStarParam param)
        {
            foreach (var handler in Handlers[StarPhase.Unload])
            {
                handler.Invoke(param);
            }
            StarGate.RemoveStar(this);
        }

        protected StarResult InvokePhase(StarPhase phase, IStarParam param)
        {
            foreach (var handler in Handlers[phase].TakeWhile(_ => !param.Status.IsInterrupt()))
            {
                param.Status |= handler.Invoke(param).Status;
            }
            return param.Status;
        }

        public void RegisterHandler(Functor handler, StarPhase phase) => Handlers[phase].Add(new StarHandler(handler));

        protected readonly Dictionary<StarPhase, List<StarHandler>> Handlers = new()
        {
            { StarPhase.Pre ,new List<StarHandler>()},
            { StarPhase.On ,new List<StarHandler>()},
            { StarPhase.Post ,new List<StarHandler>()},
            { StarPhase.Unload ,new List<StarHandler>()}
        };

        public async Task<StarResult> InvokeAsync(IStarParam param) => await Task.Run(() => Invoke(param));

        public string Name { get; }
        public object Key { get; }
        public IStarGate StarGate { get; }

    }
    internal class Star<T> : Star, IStar<T>
    {
        public Star(string name, object key, IStarGate starGate) : base(name, key, starGate) { }
        public new StarResult<T> Invoke(IStarParam param)
        {
            T? result = default;
            InvokePhase(StarPhase.Pre, param);
            if (!param.Status.IsInterrupt())
            {
                param.Status |= InvokePhase(StarPhase.On, param);
                foreach (var handler in _typedHandlers[StarPhase.On].TakeWhile(_ => !param.Status.IsInterrupt()))
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

        public new async Task<StarResult<T>> InvokeAsync(IStarParam param) => await Task.Run(() => Invoke(param));

        public void RegisterHandler(Functor<T> handler, StarPhase phase)
        {
            if (phase is StarPhase.On)
                _typedHandlers[phase].Add(new StarHandler<T>(handler));
            else
                Handlers[phase].Add(new StarHandler<T>(handler));
        }

        private readonly Dictionary<StarPhase, List<StarHandler<T>>> _typedHandlers = new()
        {
            { StarPhase.On ,new List<StarHandler<T>>()},
        };
    }
}

