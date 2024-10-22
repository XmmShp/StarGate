using StarGate.Abstractions;
using StarGate.Adapter;
using StarGate.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        private async Task<IList<T>> InvokePhaseAsync(StarPhase phase, IStarParam param)
        {
            var results = new List<T>();
            var tasks = _handlers[phase].Select(handler => Task.Run(() => handler.Invoke(param))).ToList();
            while (tasks.Count > 0)
            {
                var completedTask = await Task.WhenAny(tasks);
                var index = tasks.IndexOf(completedTask);
                var result = await completedTask;
                results.Add(result);
                tasks.RemoveAt(index);
            }
            return results;
        }

        private IList<T> InvokePhase(StarPhase phase, IStarParam param) => _handlers[phase].Select(handler => handler.Invoke(param)).ToList();

        public void Unload(IStarParam param)
        {
            InvokePhase(StarPhase.Unload, param);
            _starGate.RemoveStar(this);
        }

        public async Task UnloadAsync(IStarParam param)
        {
            await InvokePhaseAsync(StarPhase.Unload, param);
            _starGate.RemoveStar(this);
        }

        public IList<T> Invoke(IStarParam param)
        {
            var results = new List<T>();
            results.AddRange(InvokePhase(StarPhase.Pre, param));
            if (!param.Status.IsInterrupted())
                results.AddRange(InvokePhase(StarPhase.On, param));
            if (!param.Status.IsInterrupted())
                results.AddRange(InvokePhase(StarPhase.Post, param));
            return results;
        }

        public async Task<IList<T>> InvokeAsync(IStarParam param)
        {
            var results = new List<T>();
            results.AddRange(await InvokePhaseAsync(StarPhase.Pre, param));
            if (!param.Status.IsInterrupted())
                results.AddRange(await InvokePhaseAsync(StarPhase.On, param));
            if (!param.Status.IsInterrupted())
                results.AddRange(await InvokePhaseAsync(StarPhase.Post, param));
            return results;
        }

        public void RegisterHandler(Functor<T> handler, StarPhase phase) => _handlers[phase].Add(new StarHandler<T>(handler));

        private readonly Dictionary<StarPhase, List<StarHandler<T>>> _handlers = new()
        {
            { StarPhase.Pre ,new List<StarHandler<T>>()},
            { StarPhase.On ,new List<StarHandler<T>>()},
            { StarPhase.Post ,new List<StarHandler<T>>()},
            { StarPhase.Unload ,new List<StarHandler<T>>()},
        };

        public string Name { get; }
        public object? Key { get; }
        private readonly IStarGate _starGate;
    }
}

