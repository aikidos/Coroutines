using System;
using System.Collections.Generic;
using System.Linq;

namespace Coroutines
{
    /// <summary>
    /// Implementation of the coroutine scheduler. 
    /// </summary>
    public sealed class CoroutineScheduler : ICoroutineScheduler
    {
        private readonly List<CoroutineDecorator> _coroutines = new List<CoroutineDecorator>();
        private readonly object _lock = new object();

        /// <inheritdoc />
        public ICoroutine Run(Func<IEnumerator<IRoutineReturn>> factory)
        {
            if (factory == null) 
                throw new ArgumentNullException(nameof(factory));

            lock (_lock)
            {
                var coroutine = new CoroutineDecorator(factory);

                _coroutines.Add(coroutine);

                return coroutine;
            }
        }

        /// <inheritdoc />
        public bool Update()
        {
            lock (_lock)
            {
                var finishedCoroutines = _coroutines
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

                foreach (var coroutine in finishedCoroutines)
                {
                    coroutine.Dispose();
                    _coroutines.Remove(coroutine);
                }

                return _coroutines.Count > 0;
            }
        }

        /// <inheritdoc />
        public void WaitAll()
        {
            while (Update()) 
            { }
        }

        /// <inheritdoc />
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
