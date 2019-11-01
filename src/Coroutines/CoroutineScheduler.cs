using System;
using System.Collections.Generic;
using System.Linq;

namespace Coroutines
{
    public sealed class CoroutineScheduler<TContextValue> : ICoroutineScheduler<TContextValue>
    {
        private readonly List<Coroutine> _coroutines = new List<Coroutine>();
        private readonly Dictionary<Coroutine, IAwaitReturn> _pending = new Dictionary<Coroutine, IAwaitReturn>();
        private readonly CoroutineContext<TContextValue> _context;
        private readonly object _lock = new object();

        public TContextValue ContextValue => _context.Value;

        public CoroutineScheduler(TContextValue contextValue)
        {
            _context = new CoroutineContext<TContextValue>
            {
                Value = contextValue
            };
        }

        public void Run(Func<CoroutineContext<TContextValue>, IEnumerator<IRoutineReturn>> factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            lock (_lock)
            {
                var coroutine = new Coroutine(() => factory(_context));

                _coroutines.Add(coroutine);
            }
        }

        public bool Update()
        {
            lock (_lock)
            {
                var garbage = _coroutines
                    .Where(coroutine =>
                    {
                        if (!coroutine.MoveNext()) 
                            return true;

                        switch (coroutine.Current)
                        {
                            case IAwaitReturn awaiter:
                                _pending.Add(coroutine, awaiter);
                                awaiter.Start();
                                return true;

                            case ResetReturn _:
                                coroutine.Reset();
                                break;
                        }

                        return false;
                    })
                    .ToArray();

                foreach (var coroutine in garbage)
                {
                    coroutine.Dispose();
                    _coroutines.Remove(coroutine);
                }

                foreach (var (coroutine, awaiter) in _pending
                    .Where(pair => pair.Value.IsFinished))
                {
                    awaiter.Dispose();

                    _pending.Remove(coroutine);
                    _coroutines.Add(coroutine);
                }

                return _coroutines.Count > 0 || _pending.Count > 0;
            }
        }

        public void WaitAll()
        {
            while (Update()) { }
        }

        public void Dispose()
        {
            lock (_lock)
            {
                foreach (var coroutine in _coroutines)
                {
                    coroutine.Dispose();
                }

                foreach (var (coroutine, awaiter) in _pending)
                {
                    awaiter.Dispose();
                    coroutine.Dispose();
                }

                _coroutines.Clear();
                _pending.Clear();
            }
        }
    }
}
