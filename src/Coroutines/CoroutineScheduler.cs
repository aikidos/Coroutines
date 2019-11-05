using System;
using System.Collections.Generic;
using System.Linq;

namespace Coroutines
{
    public sealed class CoroutineScheduler : ICoroutineScheduler
    {
        private readonly List<CoroutineDecorator> _coroutines = new List<CoroutineDecorator>();
        private readonly object _lock = new object();

        public void Run(Func<IEnumerator<IRoutineReturn>> factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            lock (_lock)
            {
                var coroutine = new CoroutineDecorator(factory);

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
                        if (coroutine.IsAwaiting)
                            return false;

                        if (!coroutine.MoveNext()) 
                            return true;

                        switch (coroutine.Current)
                        {
                            case IRoutineAwaiter awaiter:
                                coroutine.Await(awaiter);
                                break;

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

                return _coroutines.Count > 0;
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

                _coroutines.Clear();
            }
        }
    }
}
